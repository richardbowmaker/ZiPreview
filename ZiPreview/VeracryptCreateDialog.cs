using System;
using System.Windows.Forms;

namespace ZiPreview
{
    public partial class VeracryptCreateDialog : Form
    {
        public static void Run()
        {
            VeracryptCreateDialog dlg = new VeracryptCreateDialog();
            dlg.Show();
        }

        public VeracryptCreateDialog()
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

}
