using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace ZiPreview
{

    public class VHD
	{
		public enum VhdType { NotAttached, Attached, Unlocked }; 
		public string Filepath { get; set; }
		public string Drive { get; set; }
		public VhdType _Type { get; set; }
		public bool BitLocked { get; set; }
	}

	public class ManageVHDs
	{
		static private List<VHD> _vhds = new List<VHD>();
		static private string _diskPart = "_ManageVhdDiskPart.txt";
		static private string _diskDetach = "Detach.bat";
        static private string _diskPartAll = "_ManageVhdDiskPartAll.txt";
        static private string _powershell = "_ManageVhdPowerShell.ps1";
        public static string ObsCaptureDir { get; set; }
        public static string FilesTargetDir { get; set; }

        public static List<VHD> VirtualDrives { get { return _vhds; } }

        static public bool AttachAllVHDs()
		{
			UnattachAllVHDs();
			_vhds = new List<VHD>();

			DriveInfo[] dis = DriveInfo.GetDrives();
			List<string> vhdFiles = new List<string>();

			foreach (DriveInfo di in dis)
			{
				DirectoryInfo root = di.RootDirectory;
				String path = root.FullName;
				
				foreach (string sp in Constants.ScanPaths)
				{
					if (Directory.Exists(path + sp))
					{
						var files = Directory.EnumerateFiles(path + sp, "*.vhd", SearchOption.AllDirectories);
                        vhdFiles.AddRange(files);
                    }
                }

				// for removable drives scan the root directory
				if (di.DriveType == DriveType.Removable)
				{
					var files = Directory.EnumerateFiles(path, "*.vhd", SearchOption.AllDirectories);
                    vhdFiles.AddRange(files);
                }
            }

			// quit if no vhd files found
			if (vhdFiles.Count == 0) return false;

            vhdFiles.Sort();

			foreach (string vhdFile in vhdFiles)
			{
				// for each vhd add the following to the script to attach it;
				//    select vdisk file = D:\_Rick's\c#\ManageVHD\TestBitLockerVol.vhd
				//    attach vdisk
				List<string> script = new List<string>();
				script.Add("select vdisk file=" + vhdFile);
				script.Add("attach vdisk");
				string sfn = Constants.WorkingFolder + "\\" + _diskPart;
				bool ok = Utilities.RunScript("diskpart /s <file>", sfn, script);
				Utilities.DeleteFile(sfn);

                if (ok)
                {
                    Logger.TraceInfo("Attached virtual drive " + vhdFile + " OK");
                }
                else
                {
                    Logger.TraceError("*** Failed to attach virtual drive " + vhdFile);
                    break;
                }

                // look for newly attached vhd drive
                DriveInfo[] dis1 = DriveInfo.GetDrives();
				bool found = false;
				foreach (DriveInfo di1 in dis1)
				{
					found = false;
					foreach (DriveInfo di in dis)
					{
						if (di1.RootDirectory.FullName == di.RootDirectory.FullName)
						{
							found = true;
							break;
						}
					}

					// the drive for the attached drive has been located
					if (!found)
					{
						VHD v = new VHD();
						v.Filepath = vhdFile;
						v.Drive = di1.RootDirectory.FullName;
						v._Type = VHD.VhdType.Attached;
						v.BitLocked = false;

						// test to see if it is bitlocked
						try
						{
							long l = di1.AvailableFreeSpace;
                            Logger.TraceError("*** drive not bitlocked");
                        }
                        catch (Exception)
						{
							v.BitLocked = true;
						}
						_vhds.Add(v);
						break;
					}
				}
				dis = dis1;
            }

			foreach (VHD vhd in _vhds)
			{
				// unlock the vhd
				if (vhd._Type == VHD.VhdType.Attached)
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
                        vhd._Type = VHD.VhdType.Unlocked;
                        Logger.TraceInfo("Drive unlocked: " + vhd.Filepath);
                    }
                    else
                    {
                        Logger.TraceError("*** drive not unlocked: " + vhd.Filepath);
                    }
                    Utilities.DeleteFile(sfn);
				}
			}

            GenerateDetachAllScript();
            return true;
		}
		static public bool UnattachAllVHDs()
		{
            bool result = true;

            foreach (VHD vhd in _vhds)
            {
                List<string> script = new List<string>();
                script.Add("select vdisk file = " + vhd.Filepath);
                script.Add("detach vdisk");
                string sfn = Constants.WorkingFolder + "\\" + _diskPart;
                bool ok = Utilities.RunScript("diskpart /s <file>", sfn, script);
                if (ok)
                {
                    vhd._Type = VHD.VhdType.NotAttached;
                    Logger.TraceInfo("Drive detached: " + vhd.Filepath);
                }
                else
                {
                    Logger.TraceError("*** drive did not detach: " + vhd.Filepath);
                    result = false;
                }
            }

			if (!result)
			{
				MessageBox.Show("Not all drives detached", Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return true;
		}

        static public string GenerateDetachAllScript()
        {
            StreamWriter sw = new StreamWriter(Constants.WorkingFolder + "\\" + _diskPartAll, false);
            foreach (VHD vhd in _vhds)
            {
                sw.WriteLine("select vdisk file = " + vhd.Filepath);
                sw.WriteLine("detach vdisk");
            }
            sw.Close();

            sw = new StreamWriter(Constants.WorkingFolder + "\\" + _diskDetach, false);
            sw.WriteLine("diskpart /s " + Constants.WorkingFolder + "\\" + _diskPartAll);
            sw.Close();

            return Constants.WorkingFolder + "\\" + _diskDetach;
        }

        static public List<string> GetAllFiles()
		{
			SortedList<string, string> fileList = new SortedList<string, string>();

			foreach (VHD vhd in _vhds)
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

                // create obs capture directory
                // the last drive iterated will be the one used
                try
                {
                    ObsCaptureDir = vhd.Drive.Substring(0, 2) + Constants.ObsCapturePath;
                    Directory.CreateDirectory(ObsCaptureDir);
                    Logger.TraceInfo("OBS capture folder: " + ObsCaptureDir);

                    // create target directory for new files
                    // the last drive iterated will be the one used
                    FilesTargetDir = vhd.Drive.Substring(0, 2) + Constants.FilesTargetPath;
                    Directory.CreateDirectory(FilesTargetDir);
                    Logger.TraceInfo("Files target folder: " + FilesTargetDir);
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to create folders: " + ObsCaptureDir + ", " + FilesTargetDir, 
                        Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            foreach (VHD vhd in _vhds)
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
                foreach (VHD vhd in _vhds)
                {
                    long fs = Utilities.GetDriveFreeSpace(vhd.Drive);

                    if (fs > bytes) return vhd.Drive;
                }
                return "";
            }
        }
    }
}
