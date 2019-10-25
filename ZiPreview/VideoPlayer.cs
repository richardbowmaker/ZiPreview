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
    public partial class VideoPlayer : Form
    {
        private static string _url;
        private static VideoPlayer _form = null;
        private static Rectangle _position = new Rectangle(-1, 0, 0, 0);

        private AxWMPLib.AxWindowsMediaPlayer _player;

        public VideoPlayer()
        {
            InitializeComponent();
        }

        public static bool IsPlaying
        {
            get { return _form != null;  }
        }

        public static void Play(string url, Form owner)
        {
            if (_form == null)
            {
                _url = url;
                _form = new VideoPlayer();
                _form.Show(owner);
            }
        }

        private void VideoPlayer_Load(object sender, EventArgs e)
        {

            Text = Constants.Title + "[" + _url + "]";

            _player = new AxWMPLib.AxWindowsMediaPlayer();
            panel.Controls.Add(_player);

            _player.Location = new Point(0, 0);
            _player.Size = panel.Size;

            _player.Dock = DockStyle.Fill;
            _player.URL = _url;
            //_player.uiMode = "none";
            _player.settings.volume = 10;

            if (_position.Left != -1)
            {
                _form.Location = new Point(_position.Left, _position.Top);
                _form.Size = new Size(_position.Width, _position.Height);
            }
        }

        private void VideoPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            _player.Ctlcontrols.stop();
            _player.URL = "";
            _player = null;
            _position = new Rectangle(_form.Location, _form.Size);
            _form = null;
        }
    }
}
