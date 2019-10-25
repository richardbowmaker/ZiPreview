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
    public partial class VerifyDelete : Form
    {
        private static FileT _file;

        private CPicture _picture;
        private AxWMPLib.AxWindowsMediaPlayer _player;
        private VideoPreview _preview = null;

        public VerifyDelete()
        {
            InitializeComponent();
        }

        public static void DoDelete(FileT file)
        {
            _file = file;
            VerifyDelete form = new VerifyDelete();
            form.ShowDialog();
        }

        private void VerifyDelete_Load(object sender, EventArgs e)
        {
            Text = Constants.Title + " Delete";

            lblFile.Text = _file.Filename;

            if (_file.HasLink) lblLink.Text = _file.Link;
            else lblLink.Text = "";

            chkImage.Enabled = _file.HasImage;
            chkImage.Checked = false;
            chkVideo.Enabled = _file.HasVideo;
            chkVideo.Checked = false;
            chkLink.Enabled = _file.HasLink;
            chkLink.Checked = false;

            if (_file.HasVideo)
            {
                _player = new AxWMPLib.AxWindowsMediaPlayer();
                 panel.Controls.Add(_player);
                _player.uiMode = "none";
                _player.Dock = DockStyle.Fill;
                _preview = new VideoPreview(_player);
                _preview.Preview(_file.VideoFilename, 10, true);
            }
            else if (_file.HasImage)
            {
                _picture = new CPicture(panel);
                _picture.LoadFile(_file.ImageFilename);
            }

            UpdateGui();
        }

        private void UpdateGui()
        {
            butDelete.Enabled = chkImage.Checked || chkVideo.Checked || chkLink.Checked;
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButDelete_Click(object sender, EventArgs e)
        {
            if (_preview != null) _preview.PreviewCancel();
            Close();
            Files.DeleteFile(_file, chkImage.Checked, chkVideo.Checked, chkLink.Checked);
        }

        private void ChkImage_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGui();
        }

        private void ChkVideo_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGui();
        }

        private void ChkLink_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGui();
        }

        private void VerifyDelete_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_preview != null)
            {
                _preview.PreviewCancel();
                _preview = null;
                _player = null;
            }
        }
    }
}
