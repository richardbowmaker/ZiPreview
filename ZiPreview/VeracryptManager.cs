using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace ZiPreview
{
    public class VeracryptVolume
    {
        public VeracryptVolume(string file, bool isSelected)
        {
            Filename = file;
            Drive = "";
            IsMounted = false;
            IsSelected = isSelected;
        }

        public override string ToString()
        {
            if (IsMounted)
            {
                long? fs = Utilities.GetDriveFreeSpace(Drive);
                string fss = "";
                if (fs.HasValue) fss = String.Format("{0:n0}", fs.Value);
                return Filename + ", " + Drive + ", " + fss + " bytes";
            }
            else return Filename;
        }

        public string Filename;
        public string Drive;
        public bool IsMounted;
        public bool IsSelected;

        public bool MountVolume(string drive)
        {
            // quit if already mounted
            if (IsMounted) return true;

            // test to see if drive is already mounted
            try
            {
                string[] s = Directory.GetFiles(drive);
                Logger.Error("Veracrypt mount, drive " + drive + " already in use");
                return false;
            }
            catch (Exception)
            {
            }

            // mount:
            //      "C:\Program Files\VeraCrypt\VeraCrypt.exe" /q /a /hash sha512 /v VolAccounts.hc /l x /p password
            string cmd = Constants.VeracryptExe;
            string args = "/q /a /hash sha512" + " /v \"" + Filename + "\" /l " + drive.Substring(0, 1).ToLower() + " /p " + Constants.Password;

            if (Utilities.RunCommandSync(cmd, args, 60000))
            {
                Logger.Info("Mounted Veracrypt volume \"" + Filename + "\" as " + drive);
                IsMounted = true;
                Drive = drive;

                // ensure that the files directory exists
                Utilities.MakeDirectory(drive + Constants.FilesTargetPath);

                // add to property cache
                PropertyCache.AddDirectory(drive);
                return true;
            }
            else
            {
                Logger.Error("Failed to mount Veracrypt volume \"" + Filename + "\" as " + drive);
                return false;
            }
        }

        public void Unmount()
        {
            if (IsMounted)
            {
                string cmd = Constants.VeracryptExe;
                string args = "/q /d " + Drive.Substring(0, 1).ToLower();

                if (Utilities.RunCommandSync(cmd, args, 10000))
                {
                    Logger.Info("Unmounted Veracrypt volume: " + Drive + ", " + Filename);
                    IsMounted = false;
                    Drive = "";
                }
                else
                {
                    Logger.Error("Could not unmount Veracrypt volume: " + Drive + ", " + Filename);
                }
            }
        }
    }

    public class VeracryptManager
    {
        private static List<VeracryptVolume> _volumes = new List<VeracryptVolume>();

        public static List<VeracryptVolume> Volumes { get => _volumes; }

        private static string _nextFreeDrive = "";
        public static string NextFreeDrive
        {
            get => _nextFreeDrive;
            set => _nextFreeDrive = value.Substring(0, 1).ToUpper() + "\\";
        }

        public static void SetVolumes(List<VeracryptVolume> volumes)
        {
            // unmount volumes that are not present in the new list of volumes
            foreach (VeracryptVolume vol in _volumes)
            {
                int n = volumes.FindIndex(v => v.Filename.CompareTo(vol.Filename) == 0);
                if (n == -1)
                {
                    if (vol.IsMounted) vol.Unmount();
                }
            }

            // create new volumes list 
            foreach (VeracryptVolume vol in volumes)
            {
                // find it in the current list of volumes
                int n = _volumes.FindIndex(v => v.Filename.CompareTo(vol.Filename) == 0);
                if (n != -1)
                {
                    // if volume is no longer selected unmount it
                    if (!vol.IsSelected && vol.IsMounted) vol.Unmount();
                }
            }

            // in filename order
            volumes.Sort((v1, v2) => v1.Filename.CompareTo(v2.Filename));
            _volumes = volumes;

            MountSelectedVolumes();
        }

        public static bool FindAllVolumes()
        {
            _volumes = new List<VeracryptVolume>();

            DriveInfo[] dis = DriveInfo.GetDrives();
            List<string> volFiles = new List<string>();

            foreach (DriveInfo di in dis)
            {
                DirectoryInfo root = di.RootDirectory;
                String path = root.FullName;

                foreach (string sp in Constants.ScanPaths)
                {
                    if (Directory.Exists(path + sp))
                    {
                        var files = Directory.EnumerateFiles(path + sp, "*.hc", SearchOption.AllDirectories);
                        volFiles.AddRange(files);
                    }
                }
            }

            // quit if no hc files found
            if (volFiles.Count == 0) return false;

            volFiles.Sort();
            foreach (string volFile in volFiles)
            {
                VeracryptVolume vol = new VeracryptVolume(volFile, false);
                _volumes.Add(vol);
            }
            return true;
        }

        public static bool MountSelectedVolumes()
        {
            foreach (VeracryptVolume vol in _volumes)
            {
                if (vol.IsSelected && !vol.IsMounted)
                {
                    string drive = Utilities.GetFirstUnusedDrive();
                    if (drive == null)
                    {
                        Logger.Error("Veracrypt mount, run out of drive letters");
                        return false;
                    }
                    vol.MountVolume(drive);
                }
            }
            return true;
        }

        public static void UnmountVolumes(bool all = true)
        {
            // unmount: "C:\Program Files\VeraCrypt\VeraCrypt.exe" /q /d x
            foreach (VeracryptVolume vol in _volumes)
            {
                // if all is set, then detach all mounted volumes
                // if not all, then detach only the deselected volumes
                if ((all && vol.IsMounted) || (!all && vol.IsMounted && !vol.IsSelected))
                    vol.Unmount();
            }
        }
        static public List<string> GenerateUnmountScript()
        {
            List<string> script = new List<string>();
            _volumes.ForEach(v =>
            {
                if (v.IsMounted) script.Add
                ("\"" + Constants.VeracryptExe + "\" /q /d " + v.Drive.Substring(0, 1).ToLower());
            });
            return script;
        }

        public static bool HasMountedVolumes
        {
            get { return _volumes.FindIndex(v => v.IsMounted) != -1; }
        }

        public static List<DriveVolume> GetDrives()
        {
            List<DriveVolume> drives = new List<DriveVolume>();
            foreach (VeracryptVolume vol in _volumes)
            {
                if (vol.IsMounted)
                    drives.Add(new DriveVolume(vol.Drive, vol.Filename + " [Veracrypt]"));
 
            }
            return drives;
        }
    }
}
