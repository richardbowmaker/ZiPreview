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
		static private List<string> _scanPaths = new List<string>() { "_Ricks\\JigsawProject", "Encrypted" };
		static private string _diskPart = "_ManageVhdDiskPart.txt";
		static private string _powershell = "_ManageVhdPowerShell.ps1";
        public static IGuiUpdate GuiUpdateIf { set; private get; }
        public static string ObsCaptureDir { get; set; }

        static public bool AttachAllVHDs(string password)
		{
			UnattachAllVHDs();
			_vhds = new List<VHD>();

			DriveInfo[] dis = DriveInfo.GetDrives();
			List<string> vhdFiles = new List<string>();

			foreach (DriveInfo di in dis)
			{
				DirectoryInfo root = di.RootDirectory;
				String path = root.FullName;
				
				foreach (string sp in _scanPaths)
				{
					if (Directory.Exists(path + sp))
					{
						var files = Directory.EnumerateFiles(path + sp, "*.vhd", SearchOption.AllDirectories);
						foreach (string file in files)
						{
							vhdFiles.Add(file);
						}
					}
				}

				// for removable drives scan the root directory
				if (di.DriveType == DriveType.Removable)
				{
					var files = Directory.EnumerateFiles(path, "*.vhd", SearchOption.AllDirectories);
					foreach (string file in files)
					{
						vhdFiles.Add(file);
					}
				}
			}

			// quit if no vhd files found
			if (vhdFiles.Count == 0) return false;

			foreach (string vhdFile in vhdFiles)
			{
				// for each vhd add the following to the script to attach it;
				//    select vdisk file = D:\_Rick's\c#\ManageVHD\TestBitLockerVol.vhd
				//    attach vdisk
				List<string> script = new List<string>();
				script.Add("select vdisk file=" + vhdFile);
				script.Add("attach vdisk");
				string sfn = Directory.GetCurrentDirectory() + "\\" + _diskPart;
				bool ok = Utilities.RunScript("diskpart /s <file>", sfn, script);
				DeleteFile(sfn);

                if (ok)
                {
                    GuiUpdateIf.TraceTS("Attached virtual drive " + vhdFile + " OK");
                }
                else
                {
                    GuiUpdateIf.TraceTS("*** Failed to attach virtual drive " + vhdFile);
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
                            GuiUpdateIf.TraceTS("*** drive not bitlocked");
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
					string sfn = Directory.GetCurrentDirectory() + "\\" + _powershell;

					// powershell -executionpolicy bypass -file <file> "G:" "1234567890"
					bool ok = Utilities.RunScript(
						"powershell -executionpolicy bypass -file <file>" + 
						" \"" + vhd.Drive.Substring(0,2) + "\" \"" + password + "\"", 
						sfn, script);

                    if (ok)
                    {
                        vhd._Type = VHD.VhdType.Unlocked;
                        GuiUpdateIf.TraceTS("Drive unlocked: " + vhd.Filepath);
                    }
                    else
                    {
                        GuiUpdateIf.TraceTS("*** drive not unlocked: " + vhd.Filepath);
                    }
                    DeleteFile(sfn);
				}
			}

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
                string sfn = Directory.GetCurrentDirectory() + "\\" + _diskPart;
                bool ok = Utilities.RunScript("diskpart /s <file>", sfn, script);
                if (ok)
                {
                    vhd._Type = VHD.VhdType.NotAttached;
                    GuiUpdateIf.TraceTS("Drive detached: " + vhd.Filepath);
                }
                else
                {
                    GuiUpdateIf.TraceTS("*** drive did not detach: " + vhd.Filepath);
                    result = false;
                }
            }

			if (!result)
			{
				MessageBox.Show("Not all drives detached", Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return true;
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
                ObsCaptureDir = vhd.Drive.Substring(0, 2) + Constants.ObsCapturePath;
                Directory.CreateDirectory(ObsCaptureDir);
            }

            List<string> result = new List<string>();
			foreach (KeyValuePair<string, string> kvp in fileList)
			{
				result.Add(kvp.Value);
			}

            return result;
		}

        public static List<string> GetDrives()
        {
            List<string> ds = new List<string>();
            foreach (VHD vhd in _vhds)
            {
                ds.Add(vhd.Drive);
            }
            return ds;
        }

		static private void DeleteFile(string fn)
		{
			// delete file, no exceptions
			try
			{
				File.Delete(fn);
			}
			catch (Exception)
			{
			}
		}

	}
}
