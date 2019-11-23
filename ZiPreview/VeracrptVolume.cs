using System;
using System.IO;

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

}
