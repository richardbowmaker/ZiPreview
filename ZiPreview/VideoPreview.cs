using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZiPreview
{
    class VideoPreview
    {
        private const int kTimerInterval = 1500;     // 500 milliseconds

        private AxWMPLib.AxWindowsMediaPlayer _player;
        private Timer _timer;

        private List<int>   _clips;
        private int         _clipIx;
        private bool        _random;
        private string      _file;
        private int         _noOfClips;

        public VideoPreview(AxWMPLib.AxWindowsMediaPlayer player)
        {
            _player = player;
            _clips = new List<int>();

            // setup timer to control playback of clips
            _timer = new Timer();
            _timer.Interval = kTimerInterval;
            _timer.Enabled = false;
            _timer.Tick += new System.EventHandler(this.TickEvent);
        }

        public void Preview(string file, int noOfClips, bool random)
        {
            _random = random;
			_file = file;
            _clipIx = 0;
            _noOfClips = noOfClips;

            _timer.Interval = 500;
            _timer.Enabled = true;
        }
 
        private List<int> CalculateClips(double duration, int noOfClips, bool random)
        {
            List<int> clips = new List<int>();

            if (duration > 0.0)
            {
                // set up array of preview points, add a bit of random variation
                // around each point
                Random rnd = new Random();
                double step = duration / (double)noOfClips;

                for (double i = 0; i < noOfClips; ++i)
                {
                    double clip;

                    if (random)
                    {
                        clip = (i + (rnd.NextDouble() * 0.7)) * step;
                    }
                    else
                    {
                        clip = i * step;
                    }
                    clips.Add((int)clip);
                }
            }
            return clips;
        }

        private void NextClip()
        {
			if (_clipIx == 0)
			{
                // get duration of media
                var wmp = new WMPLib.WindowsMediaPlayer();
                var media = wmp.newMedia(_file);
                double duration = media.duration;

                _clips = CalculateClips(duration, _noOfClips, _random);

                if (_clips.Count() == 0)
                {
                    _timer.Enabled = false;
                    return;
                }

                _player.uiMode = "none";
                _player.URL = _file;
				_player.settings.mute = true;
			}

            _timer.Interval = kTimerInterval;

            // start clip
            _player.Ctlcontrols.currentPosition = _clips[_clipIx];

            // line up next clip
            _clipIx = (_clipIx + 1) % _clips.Count();
        }

        public void PreviewStop()
        {
            _timer.Enabled = false;
            _player.Ctlcontrols.stop();
			_player.settings.mute = false;
            _player.URL = "";
		}

		public void TickEvent(object sender, EventArgs e)
        {
            NextClip();
        }
    }
}
