using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace ZiPreview
{
    public class VeracryptManager
    {
        public class VeracryptVolume
        {
            public VeracryptVolume(string file)
            {
                File = file;
                Drive = "";
                IsMounted = false;
                IsSelected = true;
            }

            public override string ToString()
            {
                if (IsMounted) return File + " [" + Drive + "]";
                else return File;
            }

            public string File;
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
                string args = "/q /a /hash sha512" + " /v \"" + File + "\" /l " + drive.Substring(0, 1).ToLower() + " /p " + Constants.Password;

                if (Utilities.RunCommandSync(cmd, args, 60000))
                {
                    Logger.Info("Mounted Veracrypt volume \"" + File + "\" as " + drive);
                    IsMounted = true;
                    Drive = drive;
                    return true;
                }
                else
                {
                    Logger.Error("Failed to mount Veracrypt volume \"" + File + "\" as " + drive);
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
                        Logger.Info("Unmounted Veracrypt volume: " + Drive);
                        IsMounted = false;
                        Drive = "";
                    }
                    else
                    {
                        Logger.Error("Could not unmount Veracrypt volume: " + Drive);
                    }
                }
            }
        }

        private static List<VeracryptVolume> _volumes = new List<VeracryptVolume>();

        public static List<VeracryptVolume> Volumes { get => _volumes; }

        private static string _nextFreeDrive = "";
        public static string NextFreeDrive
        {
            get => _nextFreeDrive;
            set => _nextFreeDrive = value.Substring(0, 1).ToUpper() + "\\";
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

            // quit if no vhd files found
            if (volFiles.Count == 0) return false;

            volFiles.Sort();
            foreach (string volFile in volFiles)
            {
                VeracryptVolume vol = new VeracryptVolume(volFile);
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

        public static void UnmountVolumes()
        {
            // unmount: "C:\Program Files\VeraCrypt\VeraCrypt.exe" /q /d x
            foreach (VeracryptVolume vol in _volumes)
            {
                vol.Unmount();
            }
        }

        public static bool CreateVolume(string file, long size)
        {
            // check file doesn't already exist
            if (File.Exists(file))
            {
                Logger.Error("Veracrypt create volume, file already exists: " + file);
                return false;
            }

            // check there is anough free space
            long? fs = Utilities.GetDriveFreeSpace(file.Substring(0, 3));
            if (!fs.HasValue || fs < size + (size / 10))
            {
                Logger.Error("Veracrypt create volume, not enough disk space" + file.Substring(0, 3));
                return false;
            }

            // launch the volume creation task
            Thread tr = new Thread(() => CreateVolumeTask(file, size, Constants.Password));
            tr.Start();

            return true;
        }

        private static void CreateVolumeTask(string file, long size, string password)
        {
            // format the size
            string sz = "";
            if (size > 10 * 1000 * 1000000L) sz = (size / (1000 * 1000000L)).ToString() + "G";
            else if (size > 10 * 1000000L) sz = (size / (1000000L)).ToString() + "M";
            else sz = size.ToString();

            // create:
            //      "C:\Program Files\VeraCrypt\VeraCrypt Format.exe" /create d:\Encrypted\test1.hc /password perfoaltze0 /hash sha512 
            //                  /encryption AES /filesystem NTFS /size 10M /force /silent
            string cmd = Constants.VeracryptFormatExe;
            string args = "/create " + file + " /password " + password + " /hash sha512 " +
                "/encryption AES /filesystem NTFS /size " + sz + " /force /silent";

            Logger.Info("Starting creation of Veracrypt volume " + file + " size = " + sz);

            if (Utilities.RunCommandSync(cmd, args, Int32.MaxValue))
            {
                Logger.Info("Veracrypt volume " + file + " created OK");
                frmZiPreview.GuiUpdateIf.MessageBoxTS("Veracrypt volume " + file + " created OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Logger.Error("Veracrypt volume creation " + file + " failed");
            }
        }

        public static List<string> GetDrives()
        {
            List<string> drives = new List<string>();
            foreach (VeracryptVolume vol in _volumes)
            {
                if (vol.IsMounted) drives.Add(vol.Drive);
            }
            return drives;
        }
    }
}
