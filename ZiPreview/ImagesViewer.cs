using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ZiPreview
{
    interface ImagesViewerData
    {
        int GetNoOfImages();
        int GetNoOfRows();
        int GetNoOfCols();
        string GetThumbNail(int n);
        string GetVideo(int n);
    }

    class ImagesViewer
    {
        private int _kBorder = 5;

        private Panel _panel;
        private ImagesViewerData _data;
        private int _top;
        private int _noOfCells;
        private AxWMPLib.AxWindowsMediaPlayer _player;
        private Timer _timer;
        private const int kTimerInterval = 1500;     // 500 milliseconds
        private int _selected;

        private int _playing;
        VideoPreview _preview;

        public ImagesViewer(Panel panel, ImagesViewerData data)
        {
            _panel = panel;
            _data = data;
            _top = 0;
            _noOfCells = 0;

            // setup timer to control playback of clips
            _timer = new Timer();
            _timer.Interval = 250;
            _timer.Enabled = false;
            _timer.Tick += new System.EventHandler(this.TickEvent);

            _player = new AxWMPLib.AxWindowsMediaPlayer();

            // to set uimode player give player a temporary parent
            _panel.Controls.Add(_player);
            _player.uiMode = "none";
            _panel.Controls.Remove(_player);
            _player.Visible = false;

            _preview = new VideoPreview(_player);
        }

        public void Initialise()
        {
            _panel.Controls.Clear();

            // add panel for each row/col with an embedded picture box and border
            for (int r = 0; r < _data.GetNoOfRows(); ++r)
            {
                for (int c = 0; c < _data.GetNoOfCols(); ++c)
                {
                    // add panel at row column position
                    Panel p = new Panel();
                    _panel.Controls.Add(p);

                    // add picture box with border
                    PictureBox box = new PictureBox();
                    p.Controls.Add(box);
                }
            }

            _noOfCells = _data.GetNoOfRows() * _data.GetNoOfCols();
            SetSizes();
            _top = 0;
            _panel.Resize += new System.EventHandler(PanelResize);
            _selected = -1;
            _playing = -1;
        }

        public int Top {  get { return _top; } }

        public void StopPreview()
        {
            StopPlay();
        }

        public void Draw(int top)
        {
            _top = top;

            if ((_top + _noOfCells > _data.GetNoOfImages()))
            {
                _top = _data.GetNoOfImages() - _noOfCells;
                if (_top < 0) _top = 0;
            }

            for (int r = 0; r < _data.GetNoOfRows(); ++r)
            {
                for (int c = 0; c < _data.GetNoOfCols(); ++c)
                {
                    int n = r * _data.GetNoOfCols() + c;
                    PictureBox box = (PictureBox)_panel.Controls[n].Controls[0];
                    box.SizeMode = PictureBoxSizeMode.Zoom;

                    if (_top + n < _data.GetNoOfImages())
                    {
                        box.ImageLocation = _data.GetThumbNail(_top + n);
                    }
                    else
                    {
                        box.ImageLocation = "";
                    }
                }
            }
        }

        public void SetSelected(int n)
        {
            StopPlay(); 

            if (!IsVisible(n)) Draw(n);

            for (int i = 0; i < _noOfCells; ++i)
            {
                Panel p = (Panel)_panel.Controls[i];

                if (i == (n - _top)) p.BackColor = Color.Blue;
                else p.BackColor = Color.White;
            }

            _timer.Enabled = true;
            _selected = n;
        }

        public bool IsVisible(int n)
        {
            return (n >= _top) && (n < _top + _noOfCells);
        }

        private void PanelResize(object sender, EventArgs e)
        {
            SetSizes();
        }

        private void SetSizes()
        {
            for (int r = 0; r < _data.GetNoOfRows(); ++r)
            {
                for (int c = 0; c < _data.GetNoOfCols(); ++c)
                {
                    // calculate coords of each panel

                    int pw = _panel.Width / _data.GetNoOfCols();
                    int ph = _panel.Height / _data.GetNoOfRows();
                    int pt = r * ph;
                    int pl = c * pw;

                    Panel p = (Panel)_panel.Controls[r * _data.GetNoOfCols() + c];
                    p.Location = new Point(pl, pt);
                    p.Size = new Size(pw, ph);

                    PictureBox box = (PictureBox)p.Controls[0];
                    box.Location = new Point(_kBorder, _kBorder);
                    box.Size = new Size(pw - _kBorder * 2, ph - _kBorder * 2);
                }
            }
        }

        private void TickEvent(object sender, EventArgs e)
        {
            _timer.Enabled = false;

            if (_selected != -1)
            {
                StartPlay(_selected);
            }
        }

        private void StartPlay(int n)
        {
            string vid = _data.GetVideo(n);

            if (vid.Length > 0)
            {
                Panel p = (Panel)_panel.Controls[_selected - _top];
                PictureBox box = (PictureBox)p.Controls[0];
                p.Controls.Add(_player);
                _player.Location = new Point(0, 0);
                _player.Size = p.Size;
                _player.Visible = true;
                _preview.Preview(vid, 10, true);
                _playing = _selected;
                box.Visible = false;
            }
        }

        private void StopPlay()
        {
            if (_playing != -1)
            {
                _preview.PreviewCancel();
                _player.Visible = false;
                Panel p = (Panel)_panel.Controls[_selected - _top];
                PictureBox box = (PictureBox)p.Controls[0];
                box.Visible = true;
                p.Controls.Remove(_player);
                _playing = -1;
            }
        }
    }

}
