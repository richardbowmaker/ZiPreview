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

            List<VeracryptManager.VeracryptVolume> vols = VeracryptManager.Volumes;
            chksVeracrypt.Items.Clear();

            foreach (VeracryptManager.VeracryptVolume vol in vols)
                chksVeracrypt.Items.Add(vol, vol.IsSelected);
        }

        private void ButOK_Click(object sender, EventArgs e)
        {
            VeracryptManager.MountSelectedVolumes();
            frmZiPreview.GuiUpdateIf.RedrawGrid();
            Close();
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChksVeracrypt_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            VeracryptManager.VeracryptVolume vol = 
                (VeracryptManager.VeracryptVolume)chksVeracrypt.Items[e.Index];

            // if already mounted cannot unselect it
            if (vol.IsMounted) e.NewValue = CheckState.Checked;
            // set selected status
            else vol.IsSelected = e.NewValue == CheckState.Checked;
        }
    }
}
