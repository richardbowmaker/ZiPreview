using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace ZiPreview
{
    public partial class SelectDrives : Form
    {
        private static bool _ok = false;
        private static string _filter = "";
        private static string _folder = "";
        private static string _password = "";
        //private static string _filter = "*.hc";
        //private static string _folder = @"D:\_Ricks\c#\ZiPreview\EncryptedTest";
        //private static string _password = "dummypassword";

        public static bool Run()
        {
            SelectDrives frm = new SelectDrives();
            frm.ShowDialog();
            return _ok;
        }

        public SelectDrives()
        {
            InitializeComponent();
        }

        private void SelectDrives_Load(object sender, EventArgs e)
        {
            MinimumSize = Size;
            MaximumSize = Size;
            Text = Constants.Title + " Select Drives";
            txtFolder.Text = _folder;
            txtFilter.Text = _filter;
            txtPassword.Text = _password;

            // populate the check box list with the current
            // list of options held by the Veracrypt manager
            VeracryptManager.Volumes.ForEach(v => chkFiles.Items.Add(v, v.IsMounted));

            UpdateGUI();
        }

        private void butFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (_folder.Length > 0) dlg.SelectedPath = _folder;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _folder = dlg.SelectedPath;
                txtFolder.Text = dlg.SelectedPath;
            }
            UpdateGUI();
        }

        private void UpdateGUI()
        {
            butOk.Enabled =
                chkFiles.Items.Count > 0 &&
                txtPassword.Text.Length > 0;

            butFind.Enabled =
                txtFolder.Text.Length > 3 &&
                txtFilter.Text.Length > 0;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            UpdateGUI();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            UpdateGUI();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            _ok = false;
            Close();
        }

        private void butOk_Click(object sender, EventArgs e)
        {
            _folder = txtFolder.Text;
            _filter = txtFilter.Text;
            _password = txtPassword.Text;
            _ok = true;

            List<VeracryptVolume> vols = new List<VeracryptVolume>();
            for (int i = 0; i < chkFiles.Items.Count; ++i)
            {
                VeracryptVolume v = (VeracryptVolume)chkFiles.Items[i];
                v.IsSelected = chkFiles.GetItemChecked(i);
                vols.Add(v);
            }

            Constants.Password = _password;

            Logger.Info("Select drives update");
            Cursor.Current = Cursors.WaitCursor;

            // write out the properties and clear them
            FileManager.WriteProperties();

            // mount volumes and redisplay
            VeracryptManager.SetVolumes(vols);

            frmZiPreview.GuiUpdateIf.PopulateGrid();

            Cursor.Current = Cursors.Default;
            Close();
        }

        private void butFind_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            chkFiles.Items.Clear();

            string[] files = Directory
                .GetFiles(txtFolder.Text, txtFilter.Text, SearchOption.AllDirectories);

            if (files.Length == 0) return;

            // sort found volumes into filename order
            List<VeracryptVolume> vols = new List<VeracryptVolume>();
            Array.ForEach(files, file => vols.Add(new VeracryptVolume(file, true)));
            vols.Sort((v1, v2) => v1.Filename.CompareTo(v2.Filename));

            // set up check list
            List<VeracryptVolume> oldvols = VeracryptManager.Volumes;

            foreach (VeracryptVolume vol in vols)
            {
                // is the volume already being managed by the Veracrypt manager
                int n = oldvols.FindIndex(v => v.Filename.CompareTo(vol.Filename) == 0);

                if (n != -1)
                {
                    // copy over the mounted state
                    vol.IsMounted = oldvols[n].IsMounted;
                    vol.Drive = oldvols[n].Drive;
                    vol.IsSelected = oldvols[n].IsSelected;
                }
                else
                {
                    vol.IsSelected = true;
                }
                chkFiles.Items.Add(vol, vol.IsSelected);
            }
            Cursor.Current = Cursors.Default;

            UpdateGUI();
        }

        private void butSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkFiles.Items.Count; ++i)
                chkFiles.SetItemCheckState(i, CheckState.Checked);
        }

        private void butDeselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkFiles.Items.Count; ++i)
                chkFiles.SetItemCheckState(i, CheckState.Unchecked);
        }
    }
}
