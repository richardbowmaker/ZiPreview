using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZiPreview
{
    public partial class SelectVolumes : Form
    {
        public static void Run()
        {
            SelectVolumes dlg = new SelectVolumes();
            dlg.Show();
        }

        public SelectVolumes()
        {
            InitializeComponent();
        }

        private void SelectVolumes_Load(object sender, EventArgs e)
        {
            Text = Constants.Title + " Select volumes";
            MinimumSize = Size;

            // populate Veracrypt volumes
            chksVeracrypt.Items.Clear();
            foreach (VeracryptVolume vol in VeracryptManager.Volumes)
                chksVeracrypt.Items.Add(vol, vol.IsSelected);

            // populate Bitlocker volumes
            chksBitLocker.Items.Clear();
            foreach (VhdVolume vol in VhdManager.Volumes)
                chksBitLocker.Items.Add(vol, vol.IsSelected);
        }

        private void ButOK_Click(object sender, EventArgs e)
        {
            Close();
            frmZiPreview.GuiUpdateIf.PopulateGrid();
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chksBitLocker.Items.Count; ++i)
                chksBitLocker.SetItemChecked(i, true);

            for (int i = 0; i < chksVeracrypt.Items.Count; ++i)
                chksVeracrypt.SetItemChecked(i, true);
        }

        private void ButDeselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chksBitLocker.Items.Count; ++i)
                chksBitLocker.SetItemChecked(i, false);

            for (int i = 0; i < chksVeracrypt.Items.Count; ++i)
                chksVeracrypt.SetItemChecked(i, false);
        }

        private void ChksVeracrypt_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            VeracryptVolume vol = (VeracryptVolume)chksVeracrypt.Items[e.Index];
            vol.IsSelected = e.NewValue == CheckState.Checked;
        }

        private void ChksBitLocker_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            VhdVolume vol = (VhdVolume)chksBitLocker.Items[e.Index];
            vol.IsSelected = e.NewValue == CheckState.Checked;
        }
    }
}
