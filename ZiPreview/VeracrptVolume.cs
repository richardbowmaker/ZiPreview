using System;
using System.IO;

namespace ZiPreview
{
    public class VeracryptVolume
    {
        public string Filename;     // the veracrypt filename
        public string Drive;        // the drive it is mounted to, e.g. H:\
        public bool IsMounted;
        public bool IsSelected;
        public bool IsDirty;
        public DateTime TimeStamp;

        public VeracryptVolume(string file, bool isSelected)
        {
            Filename = file;
            Drive = "";
            IsMounted = false;
            IsSelected = isSelected;
            IsDirty = false;
        }

        public override string ToString()
        {
            if (IsMounted)
            {
                string s = Filename + ", " + Drive;
                long? fs = Utilities.GetDriveFreeSpace(Drive);
                if (fs.HasValue) s += ", " + Utilities.BytesToString(fs.Value);
                s += (IsDirty ? " *" : "");
                s += ", " + TimeStamp.ToString("dd-MM-yyyy HH:mm:ss");
                return s;
            }
            else return Filename;
        }

        public long? FreeSpace
        {
            get
            {
                long? fs = null;
                if (IsMounted)
                {
                    DriveInfo drive = new DriveInfo(Drive);
                    if (drive.IsReady) fs = drive.TotalFreeSpace;
                }
                return fs;
            }
        }

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
            //      "C:\Program Files\VeraCrypt\VeraCrypt.exe" /q /a /nowaitdlg y /hash sha512 /v VolAccounts.hc /l x /p password
            string cmd = Constants.VeracryptExe;
            string args = "/q /a /nowaitdlg y /hash sha512" + " /v \"" + Filename + "\" /l " + drive.Substring(0, 1).ToLower() + " /p " + Constants.Password;
            string wdir = Constants.WorkingFolder;

            if (Utilities.RunCommandSync(cmd, wdir, args, 60000))
            {
                Logger.Info("Mounted Veracrypt volume \"" + Filename + "\" as " + drive);
                IsMounted = true;
                Drive = drive;

                // ensure that the files directory exists
                Utilities.MakeDirectory(drive + Constants.FilesTargetPath);

                IsDirty = false;
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
                string args = "/q /nowaitdlg y /force /d " + Drive.Substring(0, 1).ToLower();

                if (Utilities.RunCommandSync(cmd, Constants.WorkingFolder, args, 10000))
                {
                    Logger.Info("Unmounted Veracrypt volume: " + Drive + ", " + Filename);
                    IsMounted = false;
                    Drive = "";
  
                    File.SetCreationTime(Filename, TimeStamp);
                    File.SetLastWriteTime(Filename, TimeStamp);
                    File.SetLastAccessTime(Filename, TimeStamp);
                }
                else
                {
                    Logger.Error("Could not unmount Veracrypt volume: " + Drive + ", " + Filename);
                }
            }
        }
    }

}
