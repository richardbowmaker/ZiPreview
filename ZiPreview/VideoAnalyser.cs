using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;

// ffmpeg commands
// get volume level: ffmpeg -i f3.mp4 -af "volumedetect" -vn -sn -dn -f null NUL
// adjust volume level: ffmpeg -i test.mp4 -af "volume=10dB" -c:v copy -c:a aac -b:a 192k testout.mp4
// compress video: ffmpeg -i firstvid.mp4 firstvidout.mp4

namespace ZiPreview
{
    public partial class VideoAnalyser : Form
    {
        private static FileSet _file;
        private System.Windows.Forms.Timer _timer;

        public static void Run(FileSet file)
        {
            VideoAnalyser frm = new VideoAnalyser();
            _file = file;
            frm.ShowDialog();
        }

        private CPicture _picture;
        private string _newImage;

        public VideoAnalyser()
        {
            InitializeComponent();
        }

        private void VideoAnalyser_Load(object sender, EventArgs e)
        {
            Text = Constants.Title + " - Video Processor - " + _file.FilenameNoPathAndExt;

            _timer = new System.Windows.Forms.Timer();
            _timer.Enabled = true;
            _timer.Interval = 1000;
            _timer.Tick += TimerTick;

            _picture = new CPicture(panelImage);
            _newImage = "";

            UpdateGui();

            // get the sound level and duration
            if (_file.VolumeDb.Length == 0)
                SetVideoProperties(_file);
            else
                SetVolumeLevels();
        }

        private void UpdateGui()
        {
            if (_file.VolumeDb.Length == 0)
            {
                lblVolumes.Text = "Getting volume levels ...";
                volUpDown.Enabled = false;
                chkAdjustVolume.Enabled = false;
            }
            else
            {
                lblVolumes.Text = "Volume levels dB";
                volUpDown.Enabled = true;
                chkAdjustVolume.Enabled = true;
            }

            if (chkRemoveAudio.Checked)
            {
                volUpDown.Enabled = false;
                chkAdjustVolume.Checked = false;
                chkAdjustVolume.Enabled = false;
            }

            butOK.Enabled =
                chkAdjustVolume.Checked ||
                chkCompress.Checked ||
                chkRemoveAudio.Checked ||
                _newImage.Length > 0;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (_file.VolumeDb.Length > 0)
            {
                _timer.Enabled = false;
                SetVolumeLevels();
                UpdateGui();
            }
        }

        private void SetVolumeLevels()
        {
            string[] vs = _file.VolumeDb.Replace("  ", " ").Split(' ');
            if (vs.Length == 2)
            {
                txtVolAverage.Text = vs[0].Trim();
                txtVolPeak.Text = vs[1].Trim();
                float maxv = (float)Convert.ToDouble(vs[0]);

                if (maxv >= -10.0 && maxv <= 10.0)
                    volUpDown.Value = (decimal)(-maxv);
                else
                    volUpDown.Value = (decimal)0.0;
            }
            else
            {
                // error
                _file.VolumeDb = "";
            }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (_newImage.Length > 0)
            {
                Utilities.MoveFile(_newImage, _file.ImageFilename);
                ZipPreview.GUI.RefreshImageTS(_file);
                Logger.Info("Changed image for: " + _file.ImageFilename);
            }

            // kick of processing thread
            Thread thread = new Thread(() => DoProcessVideo(
                chkCompress.Checked,
                chkRemoveAudio.Checked,
                chkAdjustVolume.Checked,
                (float)volUpDown.Value,
                _file));
            thread.Start();
            Close();
        }
        private void chkNewImage_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGui();
        }

        private void chkAdjustVolume_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGui();
        }

        private void chkCompress_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGui();
        }

        private void chkRemoveAudio_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGui();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            // clear up new image file
            if (_newImage.Length > 0) Utilities.DeleteFile(_newImage);
            Close();
        }
        /// <summary>
        /// processing functions
        /// </summary>
        /// <param name="file"></param>

        private static void DoProcessVideo(
            bool compress,
            bool removeAudio,
            bool adjustVol,
            float volInc,
            FileSet file)
        {
            if (removeAudio)
            {
                Logger.Info("Removing audio track from " + file.VideoFilename);

                // ffmpeg -i file.mp4 -map 0:0 -map 0:2 -acodec copy -vcodec copy new_file.mp4 
                string fin = file.VideoFilename;
                string fout = Path.GetDirectoryName(fin) + "\\" +
                    Path.GetFileNameWithoutExtension(fin) + "_temp" +
                    Path.GetExtension(fin);
                string cmd = Constants.FfmpegExe;
                string args = "-i \"" + fin + "\" " +
                    " -map 0:0 -acodec copy -vcodec copy " +
                    "\"" + fout + "\"";
                string wdir = Path.GetDirectoryName(fin);

                if (Utilities.RunCommandSync(cmd, wdir, args, 1000000))
                {
                    // switch temp file
                    Utilities.MoveFile(fout, fin);
                    file.VolumeDb = "";
                    ZipPreview.GUI.RefreshGridRowTS(file);
                }
            }

            if (adjustVol)
            {
                Logger.Info("Adjusting audio track for " + file.VideoFilename);

                // ffmpeg -i f.mp4 -af "volume=10dB" -c:v copy -c:a aac -b:a 192k f_temp.mp4
                string fin = file.VideoFilename;
                string fout = Path.GetDirectoryName(fin) + "\\" +
                    Path.GetFileNameWithoutExtension(fin) + "_temp" +
                    Path.GetExtension(fin);
                string cmd = Constants.FfmpegExe;
                string args = "-i \"" + fin + "\" " +
                    "-af \"volume=" + volInc.ToString("0.0") + "dB\" " +
                    "-c:v copy -c:a aac -b:a 192k " +
                    "\"" + fout + "\"";
                string wdir = Path.GetDirectoryName(fin);

                if (Utilities.RunCommandSync(cmd, wdir, args, 1000000))
                {
                    // switch temp file
                    Utilities.MoveFile(fout, fin);

                    // re-populate the volume levels
                    GetVolumeLevels(file);
                    ZipPreview.GUI.RefreshGridRowTS(file);
                }
            }

            if (compress)
            {

            }
        }

        public static void SetVideoProperties(FileSet file)
        {
            Thread thread = new Thread(() => SetVideoProperties_(file));
            thread.Start();
        }

        public static void SetVideoProperties_(FileSet file)
        {
            if (!file.HasVideo) return;
            GetVolumeLevels(file);
            GetDuration(file);
            ZipPreview.GUI.RefreshGridRowTS(file);
        }

        private static void GetDuration(FileSet file)
        {
            Logger.Info("Calculating video duration for " + file.VideoFilename);

            // get duration of media
            var wmp = new WMPLib.WindowsMediaPlayer();
            var media = wmp.newMedia(file.VideoFilename);
            int d = (int)media.duration;
            file.Duration =
                (d / 60).ToString() + ":" + (d % 60).ToString("00");
        }

        private static void GetVolumeLevels(FileSet file)
        {
            Logger.Info("Calculating volume levels for " + file.VideoFilename);

            // ffmpeg -i f3.mp4 -af "volumedetect" -vn -sn -dn -f null NUL
            string fbat = Constants.WorkingFolder + @"\" + file.FilenameNoPathAndExt + ".bat";
            string fout = Constants.WorkingFolder + @"\" + file.FilenameNoPathAndExt + ".txt";
            string cmd = Constants.FfmpegExe;
            string args = "-i \"" + file.VideoFilename + "\" -af " +
                "\"volumedetect\" -vn -sn -dn -f null NULL" +
                " 2> " + fout;
            string wdir = Constants.WorkingFolder;

            // create batch file with ffmpeg command
            // N.B. shelling ffmpeg.exe directly can lead to hanging on some video files
            StreamWriter sw = new StreamWriter(fbat);
            sw.WriteLine(cmd + " " + args);
            sw.Close();

            if (Utilities.RunCommandSync(fbat, wdir, "", 200000))
            {
                StreamReader sr = new StreamReader(fout);
                string vdata = sr.ReadToEnd();
                sr.Close();

                // ... max_volume: -5.6 dB ...
                string s = "";
                int n1;
                int n2 = -1;
                n1 = vdata.IndexOf("max_volume");
                if (n1 != -1) n2 = vdata.IndexOf("dB", n1);
                if (n2 > n1) s = vdata.Substring(n1 + 12, n2 - n1 - 12 - 1);

                n2 = -1;
                n1 = vdata.IndexOf("mean_volume");
                if (n1 != -1) n2 = vdata.IndexOf("dB", n1);
                if (n2 > n1) s += " " + vdata.Substring(n1 + 12, n2 - n1 - 12 - 1);

                file.VolumeDb = s;
            }
            Utilities.DeleteFile(fout);
            Utilities.DeleteFile(fbat);
        }

        private void butNewImage_Click(object sender, EventArgs e)
        {
            Logger.Info("Generating new image for " + _file.VideoFilename);

            _newImage = _file.Filename;

            if (_newImage.Length == 0 || _file.VideoFilename.Length == 0)
            {
                Logger.Error("No video for file set");
                return;
            }

            _newImage = Path.GetDirectoryName(_newImage) + "\\" +
                Path.GetFileNameWithoutExtension(_newImage) + "_.jpg";

            // get duration of media
            var wmp = new WMPLib.WindowsMediaPlayer();
            var media = wmp.newMedia(_file.VideoFilename);
            int duration = (int)media.duration;

            // calculate position in file, halfway or random
            int d = 0;
            Random r = new Random();
            d = r.Next(0, duration);

            // get preview image from middle
            string dss = (d / 60).ToString() + ":" + (d % 60).ToString();
            string dsf = (d / 60).ToString() + ":" + ((d + 1) % 60).ToString();

            string args = "-y -ss " + dss + " -to " + dsf + " -i " +
                _file.VideoFilename + " -frames 1 " + _newImage;

            // ffmpeg -y -ss 0:10 -to 0:11 -i <filename> -frames 1 <filename no ext>-%d.jpg
            Utilities.RunCommandSync(
                Constants.FfmpegExe,
                Constants.WorkingFolder,
                args);

            if (File.Exists(_newImage))
                _picture.LoadFile(_newImage);
            else
            {
                Logger.Error("*** Failed to create image for: " + _file.VideoFilename);
                Utilities.DeleteFile(_newImage);
                _newImage = "";
            }
            UpdateGui();
        }
    }
}
