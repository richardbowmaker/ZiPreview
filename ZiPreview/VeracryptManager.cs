using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace ZiPreview
{
 
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

        public static bool SetVolumes(List<VeracryptVolume> volumes)
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

            return MountSelectedVolumes();
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
                    if (!vol.MountVolume(drive)) return false;
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

        public static bool IsMountedVolume(string file)
        {
            string drive = Utilities.GetDrive(file);
            if (drive.Length == 0) return false;
            
            int n = _volumes.FindIndex(v => v.Drive.CompareTo(drive) == 0);
            bool ok = false;
            if (n != -1) ok = _volumes[n].IsMounted;
            if (!ok) MessageBox.Show("About to write file to non-mounted drive " + file,
                        Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);

            return ok;
        }

        public static void SetVolumeDirty(string file)
        {
            string drive = Utilities.GetDrive(file);
            if (drive.Length == 0) return;
            int n = _volumes.FindIndex(v => v.Drive.CompareTo(drive) == 0);
            if (n != -1)
            {
                _volumes[n].IsDirty = true;
                Logger.Info("Volume dirty: " + _volumes[n].ToString());
            }
        }
    }
}
