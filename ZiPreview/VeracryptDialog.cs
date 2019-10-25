using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace ZiPreview
{
    // Veracrypt commands :-
    // mount:
    //      "C:\Program Files\VeraCrypt\VeraCrypt.exe" /q /a /hash sha512 /v VolAccounts.hc /l x /p password
    // dismount:
    //      "C:\Program Files\VeraCrypt\VeraCrypt.exe" /q /d x
    // create:
    //      "C:\Program Files\VeraCrypt\VeraCrypt Format.exe" /create d:\Encrypted\test1.hc /password perfoaltze0 /hash sha512 
    //                  /encryption AES /filesystem NTFS /size 10M /force /silent

    public partial class VeracryptDialog : Form
    {
        public static void Run()
        {
            VeracryptDialog dlg = new VeracryptDialog();
            dlg.Show();
        }

        public VeracryptDialog()
        {
            InitializeComponent();
        }

        private void VeracryptDialog_Load(object sender, EventArgs e)
        {
            MaximumSize = Size;
            MinimumSize = Size;
            Text = Constants.Title + " Veracrypt create volume";

            cboSizeUnits.Items.Add("Bytes");
            cboSizeUnits.Items.Add("MB");
            cboSizeUnits.Items.Add("GB");
            cboSizeUnits.SelectedIndex = 2;

            txtSize.Text = "20";

            UpdateGui();
        }

        private void ButSelectFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "Veracrypt volumes (*.hc)|*.hc";
            dlg.RestoreDirectory = true;
            dlg.OverwritePrompt = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtVolume.Text = dlg.FileName;
            }
            UpdateGui();
        }

        private void UpdateGui()
        {
            butCreate.Enabled = txtSize.Text.Length > 0 && txtVolume.Text.Length > 0;
        }

        private void ButCreate_Click(object sender, EventArgs e)
        {
            long size = Convert.ToInt64(txtSize.Text);
            if (cboSizeUnits.SelectedIndex == 1) size *= 1000000L;
            if (cboSizeUnits.SelectedIndex == 2) size *= 1000 * 1000000L;

            if (!VeracryptManager.CreateVolume(txtVolume.Text, size))
            {
                MessageBox.Show("Could not create volume, see log", Constants.Title,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Close();

        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TxtVolume_TextChanged(object sender, EventArgs e)
        {
            UpdateGui();
        }

        private void TxtSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void TxtSize_TextChanged(object sender, EventArgs e)
        {
            UpdateGui();
        }
    }

    public class VeracryptManager
    {
        public class VeracryptVolume
        {
            public VeracryptVolume(string file)
            {
                _file = file;
                _drive = "";
                _mounted = false;
                _selected = true;
            }

            public override string ToString()
            {
                if (_mounted) return _file + " [" + _drive + "]";
                else return _file;
            }

            public string _file;
            public string _drive;
            public bool _mounted;
            public bool _selected;
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

                // for removable drives scan the root directory
                if (di.DriveType == DriveType.Removable)
                {
                    var files = Directory.EnumerateFiles(path, "*.hc", SearchOption.AllDirectories);
                    volFiles.AddRange(files);
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
                if (vol._selected && !vol._mounted)
                {
                    string drive = Utilities.GetFirstFreeDrive();
                    if (drive.Length == 0)
                    {
                        Logger.TraceError("Veracrypt mount, run out of drive letters");
                        return false;
                    }
                    MountVolume(vol, drive);
                }
            }
            return true;
        }

        public static bool MountVolume(VeracryptVolume vol, string drive)
        {
            // test to see if drive is already mounted
            try
            {
                string[] s = Directory.GetFiles(drive);
                Logger.TraceError("Veracrypt mount, drive " + drive + " already in use");
                return false;
            }
            catch (Exception)
            {
            }

            // mount:
            //      "C:\Program Files\VeraCrypt\VeraCrypt.exe" /q /a /hash sha512 /v VolAccounts.hc /l x /p password
            string cmd = Constants.VeracryptExe;
            string args = "/q /a /hash sha512" + " /v \"" + vol._file + "\" /l " + drive.Substring(0,1).ToLower() + " /p " + Constants.Password;

            if (Utilities.RunCommandSync(cmd, args, 60000))
            {
                Logger.TraceInfo("Mounted Veracrypt volume \"" + vol._file + "\" as " + drive);
                vol._mounted = true;
                vol._drive = drive;
                return true;
            }
            else
            {
                Logger.TraceError("Failed to mount Veracrypt volume \"" + vol._file + "\" as " + drive);
                return false;
            }
        }

        public static bool CreateVolume(string file, long size)
        {
            // check file doesn't already exist
            if (File.Exists(file))
            {
                Logger.TraceError("Veracrypt create volume, file already exists: " + file);
                return false;
            }

            // check there is anough free space
            long fs = Utilities.GetDriveFreeSpace(file.Substring(0, 3));
            if (fs < size + (size / 10))
            {
                Logger.TraceError("Veracrypt create volume, not enough disk space" + file.Substring(0, 3));
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

            Logger.TraceInfo("Starting creation of Veracrypt volume " + file + " size = " + sz);

            if (Utilities.RunCommandSync(cmd, args, Int32.MaxValue))
            {
                Logger.TraceInfo("Veracrypt volume " + file + " created OK");
                frmZiPreview.GuiUpdateIf.MessageBoxTS("Veracrypt volume " + file + " created OK", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);           
            }
            else
            {
                Logger.TraceError("Veracrypt volume creation " + file + " failed");
            }
        }
    }
}
