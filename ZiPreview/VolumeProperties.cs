using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace ZiPreview
{
    public partial class VolumeProperties : Form
    {
        private static string _filter = "";
        private static string _folder1 = @"";
        private static string _folder2 = @"";

        public static void Run()
        {
            VolumeProperties frm = new VolumeProperties();
            frm.ShowDialog();
        }

        public VolumeProperties()
        {
            InitializeComponent();
        }

        private void VolumeProperties_Load(object sender, EventArgs e)
        {
            Text = Constants.Title + " - Volume Properties";

            txtFolder1.Text = _folder1;
            txtFolder2.Text = _folder2;
            txtFilter.Text = _filter;

            datePicker.Format = DateTimePickerFormat.Custom;
            datePicker.CustomFormat = "dd/MM/yyyy";

            timePicker.Format = DateTimePickerFormat.Time;
            timePicker.CustomFormat = "hh:mm:ss";


            UpdateGUI();
        }

        private void UpdateGUI()
        {
            butFind.Enabled =
                txtFolder1.Text.Length > 0 &&
                txtFolder2.Text.Length > 0 &&
                txtFilter.Text.Length > 0;

            datePicker.Enabled = timePicker.Enabled = butSet.Enabled =
                lstVolumes.SelectedIndex != -1;
        }

        private void butFolder1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (_folder1.Length > 0) dlg.SelectedPath = _folder1;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _folder1 = dlg.SelectedPath;
                txtFolder1.Text = dlg.SelectedPath;
            }
            UpdateGUI();
        }

        private void butFolder2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (_folder2.Length > 0) dlg.SelectedPath = _folder2;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _folder2 = dlg.SelectedPath;
                txtFolder2.Text = dlg.SelectedPath;
            }
            UpdateGUI();
        }

        private void butFind_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            lstVolumes.Items.Clear();

            string[] files1 = Directory
                .GetFiles(txtFolder1.Text, txtFilter.Text, SearchOption.AllDirectories);
            if (files1.Length == 0) return;

            string[] files2 = Directory
                .GetFiles(txtFolder2.Text, txtFilter.Text, SearchOption.AllDirectories);
            if (files2.Length == 0) return;

            foreach (string file1 in files1)
            {
                // check the same exists in files 2
                int n = Array.FindIndex(files2, 
                    file2 => Path.GetFileName(file1).CompareTo(Path.GetFileName(file2)) == 0);

                if (n != -1)
                {
                    KeyValuePair<string, string> kvp =
                        new KeyValuePair<string, string>(file1, files2[n]);
                    int i = lstVolumes.Items.Add(kvp);
                }
            }
            UpdateGUI();

            Cursor.Current = Cursors.Default;

        }

        private void lstVolumes_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayDates(File1(), File2());
            UpdateGUI();
        }

        private void DisplayDates(string file1, string file2)
        { 
            DateTime dt;

            if (file1.Length == 0)
            {
                txtCreated1.Text = "";
                txtModified1.Text = "";
                txtAccessed1.Text = "";
            }
            else
            {
                dt = File.GetCreationTime(file1);
                txtCreated1.Text = dt.ToString("dd/MM/yyyy HH:mm:ss");
                dt = File.GetLastWriteTime(file1);
                txtModified1.Text = dt.ToString("dd/MM/yyyy HH:mm:ss");
                dt = File.GetLastAccessTime(file1);
                txtAccessed1.Text = dt.ToString("dd/MM/yyyy HH:mm:ss");
            }

            if (file2.Length == 0)
            {
                txtCreated2.Text = "";
                txtModified2.Text = "";
                txtAccessed2.Text = "";
            }
            else
            {
                dt = File.GetCreationTime(file2);
                txtCreated2.Text = dt.ToString("dd/MM/yyyy HH:mm:ss");
                dt = File.GetLastWriteTime(file2);
                txtModified2.Text = dt.ToString("dd/MM/yyyy HH:mm:ss");
                dt = File.GetLastAccessTime(file2);
                txtAccessed2.Text = dt.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }

        private void butSet_Click(object sender, EventArgs e)
        {
            DateTime dt = new DateTime(
                datePicker.Value.Year,
                datePicker.Value.Month,
                datePicker.Value.Day,
                timePicker.Value.Hour,
                timePicker.Value.Minute,
                timePicker.Value.Second,
                timePicker.Value.Millisecond,
                timePicker.Value.Kind);

            string file1 = ((KeyValuePair<string, string>)lstVolumes.SelectedItem).Key;
            string file2 = ((KeyValuePair<string, string>)lstVolumes.SelectedItem).Value;

            string fp1= Path.GetDirectoryName(file1);
            string fn1 = fp1 + "\\img" + dt.ToString("yyyyMMdd") + ".adi";

            string fp2 = Path.GetDirectoryName(file2);
            string fn2 = fp2 + "\\img" + dt.ToString("yyyyMMdd") + ".adi";

            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(fn1, fn2);
            lstVolumes.Items[lstVolumes.SelectedIndex] = kvp;

            Utilities.MoveFile(file1, fn1);
            Utilities.MoveFile(file2, fn2);

            File.SetCreationTime(fn1, dt);
            File.SetLastWriteTime(fn1, dt);
            File.SetLastAccessTime(fn1, dt);

            File.SetCreationTime(fn2, dt);
            File.SetLastWriteTime(fn2, dt);
            File.SetLastAccessTime(fn2, dt);

            DisplayDates(fn1, fn2);
        }

        private void txtCreated1_Click(object sender, EventArgs e)
        {
            string f = File1();
            if (f.Length > 0)
            {
                datePicker.Value = File.GetCreationTime(f);
                timePicker.Value = File.GetCreationTime(f); 
            }
        }

        private void txtModified1_Click(object sender, EventArgs e)
        {
            string f = File1();
            if (f.Length > 0)
            {
                datePicker.Value = File.GetLastWriteTime(f);
                timePicker.Value = File.GetLastWriteTime(f);
            }
        }

        private void txtAccessed1_Click(object sender, EventArgs e)
        {
            string f = File1();
            if (f.Length > 0)
            {
                datePicker.Value = File.GetLastAccessTime(f);
                timePicker.Value = File.GetLastAccessTime(f);
            }
        }

        private void txtCreated2_Click(object sender, EventArgs e)
        {
            string f = File2();
            if (f.Length > 0)
            {
                datePicker.Value = File.GetCreationTime(f);
                timePicker.Value = File.GetCreationTime(f);
            }
        }

        private void txtModified2_Click(object sender, EventArgs e)
        {
            string f = File2();
            if (f.Length > 0)
            {
                datePicker.Value = File.GetLastWriteTime(f);
                timePicker.Value = File.GetLastWriteTime(f);
            }
        }

        private void txtAccessed2_Click(object sender, EventArgs e)
        {
            string f = File2();
            if (f.Length > 0)
            {
                datePicker.Value = File.GetLastAccessTime(f);
                timePicker.Value = File.GetLastAccessTime(f);
            }
        }

        private string File1()
        {
            if (lstVolumes.SelectedIndex == -1) return "";
            KeyValuePair<string, string> kvp =
                (KeyValuePair<string, string>)lstVolumes.SelectedItem;
            return kvp.Key;
        }
        private string File2()
        {
            if (lstVolumes.SelectedIndex == -1) return "";
            KeyValuePair<string, string> kvp =
                (KeyValuePair<string, string>)lstVolumes.SelectedItem;
            return kvp.Value;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            UpdateGUI();
        }
    }
}
