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
    // requires OBS to be installed
    // install web socket plugin, see
    //      https://obsproject.com/forum/threads/timed-recordings-on-obs.99727/
    // install
    //      https://obsproject.com/forum/resources/obs-websocket-remote-control-of-obs-studio-made-easy.466/
    //      which takes you to https://github.com/Palakis/obs-websocket, 
    //      project needs compiling to produce the dll obs-websocket-dotnet.dll
    // then install
    //      https://github.com/Palakis/obs-websocket/releases (link at bottom of page)
    //
    // in OBS goto the Tools>WebSockets Server Settings, and set the password as defined in the Constants class.
    //

    class VideoCapture
    {
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

        private FileSet _file;

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

        public VideoCapture() { }

        public bool Initialise()
        {
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
                _startHotKey = new HotKeyRegister(ZipPreview.GUI.GetHwnd(), _kStartHotkeyId, KeyModifiers.Control, Keys.F8);
                _startHotKey.HotKeyPressed += HotKeyPressed;
                _stopHotKey = new HotKeyRegister(ZipPreview.GUI.GetHwnd(), _kStopHotkeyId, KeyModifiers.Control, Keys.F9);
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

        public bool IsCaptureInProgress { get { return _state != StateT.Stopped; } }

        public void StartCapture(FileSet file)
        {
            if (IsCaptureInProgress) return;

            // does the volume with the link file have sufficient space for
            // the video capture
            string drive = file.Filename.Substring(0, 3);
            long? free = Utilities.GetDriveFreeSpace(drive);

            if (free.HasValue && free > Constants.MinimumCaptureSpace)
            {
                Logger.Info("Sufficient space on source drive: " + drive);
            }
            else
            {
                // scan all mounted volumes looking for one that has a least
                // 500 MB free for video capture
                VeracryptVolume vol = VeracryptManager.
                    FindDriveWithFreeSpace(Constants.MinimumCaptureSpace);

                if (vol == null)
                {
                    MessageBox.Show("No volumes have sufficient diskspace", Constants.Title,
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                Logger.Info("Sufficient space found on another drive: " + vol.Drive);

                // the image/link files will need to be moved to the same volume
                // as the video capture volume
                if (!file.MoveFilesToVolume(vol.Drive)) return;

                // update the grid display
                ZipPreview.GUI.RefreshGridRowTS(file);

                drive = vol.Drive;
            }

            // create the capture folder
            _obsCaptureDir = drive + Constants.ObsCapturePath;
            if (!VeracryptManager.IsMountedVolume(_obsCaptureDir)) return;
            if (!Utilities.MakeDirectory(_obsCaptureDir)) return;
            Logger.Info("Capturing to folder: " + _obsCaptureDir);

            _obs.Connect(Constants.ObsConnect, Constants.ObsPassword);

            if (_obs.IsConnected)
            {
                _obs.SetVolume("Audio Output Capture", 0.9f);
                _obs.SetRecordingFolder(_obsCaptureDir);
                _file = file;

                // prompt user to start playing the video
                if (!VideoCapturePrompt.Run(file.Link)) return;

                Utilities.SetAudioLevel(10);

                // start the browser
                LaunchBrowser(file);

                // start timer state machine running
                EnableHotKeys(true);
                Utilities.UnmuteAudio();
                _ticks = 0;
                _state = StateT.WaitForStartKey;
                _timer.Enabled = true;
                Logger.Info("Starting capture of " + file.Link);
            }
            else
            {
                MessageBox.Show("OBS is not running", Constants.Title,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void StopCapture()
        {
            _timer.Enabled = false;
            if (_state == StateT.Recording)
            {
                _obs.StopRecording();
                KillBrowser();
            }
            _state = StateT.Stopped;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _ticks++;
            float secs = (float)(_ticks * _timer.Interval) / 1000.0f;
            bool secb = secs - Math.Truncate(secs) < 0.01f; // is second boundary

            // check for timeout
            if (secs > _kCaptureTimeout)
            {
                KillBrowser();
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
                            if (VideoCapturePrompt.Mute)
                                Utilities.MuteAudio();
                            else
                                Utilities.UnmuteAudio();
                            _captureLastChanged = secs;
                            _state = StateT.Recording;
                            _obs.StartRecording();
                            Logger.Info("Recording started: " + secs.ToString());
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
                                _obs.StopRecording();
                                _videoStoppedAt = secs;
                                Logger.Info("Video stopped playing: " + secs.ToString());

                                try
                                {
                                    // exception occurs if browser was already open
                                    KillBrowser();
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
                                ZipPreview.GUI.RefreshGridRowTS(_file);
                                Logger.Info("Video captured: " + _file.VideoFilename);
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
                            Utilities.UnmuteAudio();
                            Utilities.SetAudioLevel(30);
                            SoundEnded();
                        }
                    }
                    break;
                case StateT.CaptureError:
                    {
                        if (secb && ((int)secs) % 5 == 0)
                        {
                            Utilities.UnmuteAudio();
                            Utilities.SetAudioLevel(30);
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

            if (VeracryptManager.IsMountedVolume(dest))
            {
                if (Utilities.MoveFile(src, dest))
                {
                    _file.VideoFilename = dest;
                    VeracryptManager.SetVolumeDirty(dest);
                    FileInfo fi = new FileInfo(dest);
                    Logger.Info("Captured file size " +
                        Utilities.BytesToString(fi.Length));
                    Logger.Info("Total free space " +
                        Utilities.BytesToString(VeracryptManager.GetTotalFreeSpace()));
                    return true;
                }
            }
            return false;
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
                        Logger.Info("Cancelled video capture");
                        switch (_state)
                        {
                            case StateT.Recording:
                                // recording aborted, user can select wether to save recording or not
                                _obs.StopRecording();
                                KillBrowser();

                                // save file
                                if (MoveCaptureFile())
                                {
                                    _state = StateT.Stopped;
                                    ZipPreview.GUI.RefreshGridRowTS(_file);
                                    Logger.Info("Video captured: " + _file.VideoFilename);
                                }
                                else
                                {
                                    _state = StateT.CaptureError;
                                }
                                break;
                            case StateT.Countdown:
                                try
                                {
                                    KillBrowser();
                                }
                                catch (Exception)
                                {
                                }
                                _state = StateT.Stopped;
                                break;
                            case StateT.RecordingComplete:
                            case StateT.VideoStopped:
                            case StateT.CaptureError:
                            case StateT.WaitForStartKey:
                                _state = StateT.Stopped;
                                break;
                        }
                        EnableHotKeys(false);
                        if (!VideoCapturePrompt.Unmute)
                            Utilities.MuteAudio();
                        _timer.Enabled = false;
                        _obs.Disconnect();
                    }
                    break;
            }
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

        private void LaunchBrowser(FileSet file)
        {
            if (VideoCapturePrompt.LaunchBrowser)
                _browser = Utilities.LaunchBrowser(file);
            else
                Clipboard.SetText(file.Link, TextDataFormat.Text);
        }

        private void KillBrowser()
        {
            if (VideoCapturePrompt.LaunchBrowser && VideoCapturePrompt.CloseBrowser)
                _browser.Kill();
            else
                Clipboard.Clear();
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
