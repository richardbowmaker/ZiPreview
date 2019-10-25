using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OBSWebsocketDotNet;
using System.Media;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace ZiPreview
{
    class VideoCapture
    {
        // Win32 imports
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        // other constants

        private const float _kCaptureTimeout = 60.0f * 60.0f; // 1 hour timeout

        private Timer _timer;
        private long _ticks;

        private int _screenW;
        private int _screenH;

        private HotKeyRegister _startHotKey;
        private HotKeyRegister _stopHotKey;

        private const int _kStartHotkeyId = 0;
        private const int _kStopHotkeyId = 1;

        private IntPtr _hwin;
        private FileT _file;

        private OBSWebsocket _obs;
        private Process _browser;
        private string _obsCaptureDir;

        // video capture to determine when play is complete
        private byte[] _capture1 = null;
        private byte[] _capture2 = null;
        private Rectangle _region;
        private float _captureLastChanged; // last time capture changed in seconds
        private float _videoStoppedAt;

        private enum StateT
        {
            Stopped,
            WaitForStartKey,
            Countdown,
            Recording,
            Paused,
            VideoStopped,
            RecordingComplete,
            CaptureError
        }

        private StateT _state;

        public VideoCapture()
        {
        }

        public bool Initialise(Form form)
        {
            _hwin = form.Handle;

            // timer the drives the video capture process
            _timer = new Timer();
            _timer.Enabled = false;
            _timer.Interval = 500;
            _timer.Tick += TimerTick;

            // capture screen size
            Rectangle r = Screen.FromControl(new Form()).Bounds;
            _screenW = r.Width;
            _screenH = r.Height;

            _state = StateT.Stopped;

            try
            {
                _obs = new OBSWebsocket();
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("OBS DLLs missing", Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void Uninitialise()
        {
            if (_obs.IsConnected) _obs.Disconnect();
            _timer.Enabled = false;
            EnableHotKeys(false);
        }

        private void EnableHotKeys(bool enable)
        {
            if (enable)
            {
                _startHotKey = new HotKeyRegister(_hwin, _kStartHotkeyId, KeyModifiers.Control, Keys.F8);
                _startHotKey.HotKeyPressed += HotKeyPressed;
                _stopHotKey = new HotKeyRegister(_hwin, _kStopHotkeyId, KeyModifiers.Control, Keys.F9);
                _stopHotKey.HotKeyPressed += HotKeyPressed;
            }
            else
            {
                if (_startHotKey != null) _startHotKey.Unregister();
                if (_stopHotKey != null) _stopHotKey.Unregister();
                _startHotKey = null;
                _stopHotKey = null;
            }
        }

        public void StartCapture(FileT file)
        {
            /*
                 check for obs running
                 launch browser
                 message box, get user to prepare, clicks ok or cancel
                 wait for hot key
                 beep 5 times
                 bong once
                 start obs recording, set working directory
                 monitor screen capture for stopped
                 find the recorded file
                 notify client of capture
               */

            // find a vhd drive with sufficient space
            string drive = ManageVHDs.FindDiskWithFreeSpace(500 * 1000000);
            if (drive.Length == 0)
            {
                MessageBox.Show("Insufficient drive space", 
                    Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            _file = file;
            _obs.Connect(Constants.ObsConnect, Constants.ObsPassword);

            if (_obs.IsConnected)
            {
                // prompt user to start playing the video
                if (!VideoCapturePrompt.Display(file.Link)) return;

                _browser = Utilities.LaunchBrowser(file);
                _obsCaptureDir = drive + Constants.ObsCapturePath;
                _obs.SetRecordingFolder(_obsCaptureDir);

                // start timer state machine running
                EnableHotKeys(true);
                UnmuteAudio();
                _ticks = 0;
                _state = StateT.WaitForStartKey;
                _timer.Enabled = true;
                Logger.TraceInfo("Starting capture of " + file.Link);
            }
            else
            {
                MessageBox.Show("OBS is not running", Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _ticks++;
            float secs = (float)(_ticks * _timer.Interval) / 1000.0f;
            bool secb = secs - Math.Truncate(secs) < 0.01f; // is second boundary

            // check for timeout
            if (secs > _kCaptureTimeout)
            {
                _browser.Kill();
                _state = StateT.CaptureError;
            }

            switch (_state)
            {
                case StateT.Countdown:
                    {
                        if (secb && secs < 5.0f)
                        {
                            SoundBeep();
                        }
                        else if (secs == 5.0f)
                        {
                            SoundBong();
                        }
                        else if (secs == 6.0f)
                        {
                            if (VideoCapturePrompt.Mute) MuteAudio();
                            else UnmuteAudio();
                            _captureLastChanged = secs;
                            _state = StateT.Recording;
                            _obs.ToggleRecording();
                            Logger.TraceInfo("Recording started: " + secs.ToString());
                        }
                    }
                    break;
                case StateT.Recording:
                    {
                        if (_capture1 == null)
                        {
                            // define new capture region and capture it
                            Random rnd = new Random();
                            int l = rnd.Next(0, _screenW - 100);
                            int t = rnd.Next(0, _screenH - 100);
                            _region = new Rectangle(l, t, 100, 100);
                            _capture1 = CaptureRegion(_region);
                        }
                        else if (_capture2 == null)
                        {
                            _capture2 = CaptureRegion(_region);

                            // has the video in the region changed since last capture
                            if (!_capture1.SequenceEqual<byte>(_capture2))
                            {
                                _captureLastChanged = secs;
                            }
                            _capture1 = null;
                            _capture2 = null;

                            // if successive captures from the screen have not changed
                            // for 5 seconds, then video has stopped playing
                            if (secs - _captureLastChanged > 5.0f)
                            {
                                _state = StateT.VideoStopped;
                                _obs.ToggleRecording();
                                _videoStoppedAt = secs;
                                Logger.TraceInfo("Video stopped playing: " + secs.ToString());

                                try
                                {
                                    // exception occurs if browser was already open
                                    _browser.Kill();
                                }
                                catch (InvalidOperationException)
                                {
                                }
                            }
                        }
                    }
                    break;

                case StateT.VideoStopped:
                    {
                        // give obs 2 secs to wind up before moving video file
                        if (secs - _videoStoppedAt > 2.5)
                        {
                            // save file
                            if (MoveCaptureFile())
                            {
                                _state = StateT.RecordingComplete;
                                frmZiPreview.GuiUpdateIf.RefreshGridRowTS(_file);
                                Logger.TraceInfo("Video captured: " + _file.VideoFilename);
                            }
                            else
                            {
                                _state = StateT.CaptureError;
                            }
                        }
                    }
                    break;
                case StateT.RecordingComplete:
                    {
                        if (secb && ((int)secs) % 5 == 0)
                        {
                            UnmuteAudio();
                            SoundEnded();
                        }
                    }
                    break;
                case StateT.CaptureError:
                    {
                        if (secb && ((int)secs) % 5 == 0)
                        {
                            UnmuteAudio();
                            SoundError();
                        }
                    }
                    break;
            }
        }

        private bool MoveCaptureFile()
        {
            // get capture file and move it to new home
            string src = GetNewestFileInDirectory(_obsCaptureDir, "*.mp4");
            if (src.Length == 0) return false;

            if (!Utilities.WaitForFileReady(src, 10000)) return false;

            string dest = Path.GetDirectoryName(_file.LinkFilename) + "\\" +
                Path.GetFileNameWithoutExtension(_file.LinkFilename) + ".mp4";

            if (Utilities.MoveFile(src, dest))
            {
                _file.VideoFilename = dest;
                return true;
            }
            else return false;
        }

        private string GetNewestFileInDirectory(string dir, string searchPattern)
        {
            string fn = "";
            string[] fs = Directory.GetFiles(dir, searchPattern);

            if (fs.Length > 0)
            {
                DateTime newest = DateTime.MinValue;

                foreach (string f in fs)
                {
                    DateTime d = File.GetLastWriteTime(f);
                    if (d.CompareTo(newest) > 0)
                    {
                        newest = d;
                        fn = f;
                    }
                }
            }
            return fn;
        }

        private void HotKeyPressed(object sender, EventArgs e)
        {
            HotKeyRegister r = (HotKeyRegister)sender;

            switch (r.ID)
            {
                case _kStartHotkeyId:
                    _ticks = 0;
                    _state = StateT.Countdown;
                    break;
                case _kStopHotkeyId:
                    {
                        switch (_state)
                        {
                            case StateT.Recording:
                                _obs.ToggleRecording();
                                _browser.Kill();
                                string fn = GetNewestFileInDirectory(Constants.ObsCaptureDir, "*.mp4");
                                if (fn.Length > 0)
                                {
                                    System.Threading.Thread.Sleep(1000);
                                    Utilities.DeleteFile(fn);
                                }
                                break;
                            case StateT.Countdown:
                                _browser.Kill();
                                break;
                            case StateT.RecordingComplete:
                            case StateT.VideoStopped:
                            case StateT.CaptureError:
                            case StateT.WaitForStartKey:
                                break;
                        }
                        EnableHotKeys(false);
                        _state = StateT.Stopped;
                        if (!VideoCapturePrompt.Unmute) MuteAudio();
                        _timer.Enabled = false;
                        _obs.Disconnect();
                    }
                    break;
            }
        }

        private void MuteAudio()
        {
            UnmuteAudio();
            SendMessageW(_hwin, WM_APPCOMMAND, _hwin, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        private void UnmuteAudio()
        {
            SendMessageW(_hwin, WM_APPCOMMAND, _hwin, (IntPtr)APPCOMMAND_VOLUME_DOWN);
            SendMessageW(_hwin, WM_APPCOMMAND, _hwin, (IntPtr)APPCOMMAND_VOLUME_UP);
        }

        private byte[] CaptureRegion(Rectangle region)
        {
            byte[] bs;

            // Create a new Bitmap object
            Bitmap captureBitmap = new Bitmap(region.Width, region.Height, PixelFormat.Format32bppArgb);

            //Creating a New Graphics Object
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);

            //Copying Image from The Screen
            captureGraphics.CopyFromScreen(region.Left, region.Top, 0, 0, region.Size);

            var bitmapData = captureBitmap.LockBits(new Rectangle(0, 0, region.Width, region.Height),
            ImageLockMode.ReadOnly, captureBitmap.PixelFormat);
            var length = bitmapData.Stride * bitmapData.Height;

            // Copy bitmap to byte[]
            bs = new byte[length];
            Marshal.Copy(bitmapData.Scan0, bs, 0, length);
            captureBitmap.UnlockBits(bitmapData);

            return bs;
        }

        private void SoundBeep()
        {
            SoundPlayer sound = new SoundPlayer(Constants.BeepWav);
            sound.Play();
        }
        private void SoundBong()
        {
            SoundPlayer sound = new SoundPlayer(Constants.BongWav);
            sound.Play();
        }
        private void SoundEnded()
        {
            SoundPlayer sound = new SoundPlayer(Constants.EndedWav);
            sound.Play();
        }
        private void SoundError()
        {
            SoundPlayer sound = new SoundPlayer(Constants.ErrorWav);
            sound.Play();
        }
    }
}
