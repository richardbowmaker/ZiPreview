using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace ZiPreview
{
    public class VhdVolume
	{
        public VhdVolume(string file)
        {
            Filepath = file;
            Drive = "";
            _Type = VhdType.NotAttached;
            IsBitLocked = false;
            IsSelected = true;
        }

        public enum VhdType { NotAttached, Attached, Unlocked }; 
		public string Filepath { get; set; }
		public string Drive { get; set; }
		public VhdType _Type { get; set; }
		public bool IsBitLocked { get; set; }

        public bool IsSelected { get; set; }

        public override string ToString()
        {
            if (_Type == VhdType.Unlocked) return Filepath + " [" + Drive + "]";
            else return Filepath;
        }
    }

    public class VhdManager
	{
		static private List<VhdVolume> _volumes = new List<VhdVolume>();
		static private string _diskPart = "_ManageVhdDiskPart.txt";
        static private string _diskPartAll = "_ManageVhdDiskPartAll.txt";
        static private string _powershell = "_ManageVhdPowerShell.ps1";
        public static string ObsCaptureDir { get; set; }
        public static string FilesTargetDir { get; set; }

        public static List<VhdVolume> Volumes { get { return _volumes; } }

        public static bool FindAllVolumes()
        {
            UnmountVolumes();
            _volumes = new List<VhdVolume>();

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
                        var files = Directory.EnumerateFiles(path + sp, "*.vhd", SearchOption.AllDirectories);
                        volFiles.AddRange(files);
                    }
                }
            }

            // quit if no vhd files found
            if (volFiles.Count == 0) return false;

            volFiles.Sort();

            foreach (string volFile in volFiles)
            {
                VhdVolume vol = new VhdVolume(volFile);
                _volumes.Add(vol);
            }
            return true;
        }

        public static bool MountSelectedVolumes()
        {
            UnmountVolumes(false);

            DriveInfo[] dsOld = DriveInfo.GetDrives();

            foreach (VhdVolume vol in _volumes)
			{
                if (vol.IsSelected)
                {
                    // for each vhd add the following to the script to attach it;
                    //    select vdisk file = D:\_Rick's\c#\ManageVHD\TestBitLockerVol.vhd
                    //    attach vdisk
                    List<string> script = new List<string>();
                    script.Add("select vdisk file=" + vol.Filepath);
                    script.Add("attach vdisk");
                    string sfn = Constants.WorkingFolder + "\\" + _diskPart;
                    bool ok = Utilities.RunScript("diskpart /s <file>", sfn, script);
                    Utilities.DeleteFile(sfn);

                    if (ok)
                    {
                        Logger.Info("Attached virtual drive " + vol.Filepath + " OK");
                    }
                    else
                    {
                        Logger.Error("*** Failed to attach virtual drive " + vol.Filepath);
                        break;
                    }

                    // look for newly attached vhd drive
                    DriveInfo[] dsNew = DriveInfo.GetDrives();
                    foreach (DriveInfo di in dsNew)
                    {
                        // scan new drive list
                        int n = Array.FindIndex(dsOld, 
                            d => di.RootDirectory.FullName == d.RootDirectory.FullName);

                        // the drive for the attached drive has been located
                        if (n == -1)
                        {
                            vol.Drive = di.RootDirectory.FullName;
                            vol._Type = VhdVolume.VhdType.Attached;
                            vol.IsBitLocked = false;

                            // test to see if it is bitlocked
                            try
                            {
                                long l = di.AvailableFreeSpace;
                                Logger.Error("*** drive not bitlocked");
                            }
                            catch (Exception)
                            {
                                vol.IsBitLocked = true;
                            }
                            break;
                        }
                    }
                    dsOld = dsNew;
                }
            }

			foreach (VhdVolume vhd in _volumes)
			{
				// unlock the vhd
				if (vhd._Type == VhdVolume.VhdType.Attached)
				{
					// create power shell script to unlock drive;
					//     $SecureString = ConvertTo-SecureString $args[1] -AsPlainText -Force
					//     Unlock-BitLocker -MountPoint $args[0] -Password $SecureString
					List<string> script = new List<string>();
					script.Add("$SecureString = ConvertTo-SecureString $args[1] -AsPlainText -Force");
					script.Add("Unlock-BitLocker -MountPoint $args[0] -Password $SecureString");
					string sfn = Constants.WorkingFolder + "\\" + _powershell;

					// powershell -executionpolicy bypass -file <file> "G:" "1234567890"
					bool ok = Utilities.RunScript(
						"powershell -executionpolicy bypass -file <file>" + 
						" \"" + vhd.Drive.Substring(0,2) + "\" \"" + Constants.Password + "\"", 
						sfn, script);

                    if (ok)
                    {
                        vhd._Type = VhdVolume.VhdType.Unlocked;
                        Logger.Info("Drive unlocked: " + vhd.Filepath);
                    }
                    else
                    {
                        Logger.Error("*** drive not unlocked: " + vhd.Filepath);
                    }
                    Utilities.DeleteFile(sfn);
				}
			}
            return true;
		}
		static public bool UnmountVolumes(bool all = true)
		{
            bool result = true;

            foreach (VhdVolume vhd in _volumes)
            {
                // if all is set, then detach all attached volumes
                // if not all, then detech only the deselected volumes
                if ((all && vhd._Type != VhdVolume.VhdType.NotAttached) ||
                     (!all && vhd._Type != VhdVolume.VhdType.NotAttached && !vhd.IsSelected))
                {
                    List<string> script = new List<string>();
                    script.Add("select vdisk file = " + vhd.Filepath);
                    script.Add("detach vdisk");
                    string sfn = Constants.WorkingFolder + "\\" + _diskPart;
                    bool ok = Utilities.RunScript("diskpart /s <file>", sfn, script);
                    if (ok)
                    {
                        vhd._Type = VhdVolume.VhdType.NotAttached;
                        Logger.Info("Drive detached: " + vhd.Filepath);
                    }
                    else
                    {
                        Logger.Error("*** drive did not detach: " + vhd.Filepath);
                        result = false;
                    }
                }
            }

			if (!result)
			{
				MessageBox.Show("Not all drives detached", Constants.Title, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return true;
		}

        static public List<string> GenerateUnmountScript()
        {
            StreamWriter sw = new StreamWriter(Constants.WorkingFolder + "\\" + _diskPartAll, false);
            foreach (VhdVolume vhd in _volumes)
            {
                if (vhd._Type != VhdVolume.VhdType.NotAttached)
                {
                    sw.WriteLine("select vdisk file = " + vhd.Filepath);
                    sw.WriteLine("detach vdisk");
                }
            }
            sw.Close();

            List<string> script = new List<string>();
            script.Add("diskpart /s " + Constants.WorkingFolder + "\\" + _diskPartAll);
            return script;
        }

        public static bool HasMountedVolumes
        {
            get { return _volumes.FindIndex(v => v._Type != VhdVolume.VhdType.NotAttached) != -1; }
        }

        static public List<string> GetAllFiles()
		{
			SortedList<string, string> fileList = new SortedList<string, string>();

			foreach (VhdVolume vhd in _volumes)
			{
				string[] files = {};

				for (int i = 0; i < 10; ++i)
				{
					try
					{
						files = Directory.GetFiles(vhd.Drive.Substring(0, 2) + "\\Files", "*.*", SearchOption.AllDirectories);
						break;
					}
					catch (Exception)
					{
					}
				}

				foreach (string fn in files)
				{
					string s = Path.GetDirectoryName(fn).Substring(2) + "\\" + Path.GetFileName(fn)
									+ fileList.Count.ToString();
					fileList.Add(s, fn);
				}
            }

            List<string> result = new List<string>();
			foreach (KeyValuePair<string, string> kvp in fileList)
			{
				result.Add(kvp.Value);
			}

            return result;
		}

        static public List<string> GetDrives()
        {
            List<string> ds = new List<string>();
            foreach (VhdVolume vhd in _volumes)
            {
                ds.Add(vhd.Drive);
            }
            return ds;
        }

        static public string FindDiskWithFreeSpace(long bytes)
        {
            if (Constants.TestMode)
            {
                return Constants.WorkingFolder;
            }
            else
            {
                foreach (VhdVolume vhd in _volumes)
                {
                    long? fs = Utilities.GetDriveFreeSpace(vhd.Drive);

                    if (fs.HasValue && fs > bytes) return vhd.Drive;
                }
                return "";
            }
        }
    }
}
