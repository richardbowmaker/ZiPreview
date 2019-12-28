using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;


namespace ZiPreview
{
    public partial class VideoAnalyser : Form
    {
        private static FileSet _file;
        public static void Run(FileSet file)
        {
            VideoAnalyser frm = new VideoAnalyser();
            _file = file;
            frm.ShowDialog();
        }

        public VideoAnalyser()
        {
            InitializeComponent();
        }

        private void VideoAnalyserX_Load(object sender, EventArgs e)
        {
            Text = Constants.Title + " - Video Analyser - " + _file.Filename;


        }

        public static void SetVideoProperties(FileSet file)
        {
            Thread thread = new Thread(() => SetVideoProperties_(file));
            thread.Start();
        }

        public static void SetVideoProperties_(FileSet file)
        {
            if (!file.HasVideo) return;
            Logger.Info("Calculating video properties for " + file.VideoFilename);
            GetMaxVolume(file);
            GetDuration(file);
            ZipPreview.GUI.RefreshGridRowTS(file);
        }

        private static void GetDuration(FileSet file)
        {
            // get duration of media
            var wmp = new WMPLib.WindowsMediaPlayer();
            var media = wmp.newMedia(file.VideoFilename);
            int d = (int)media.duration;
            file.Duration =
                (d / 60).ToString() + ":" + (d % 60).ToString("00");
        }

        private static void GetMaxVolume(FileSet file)
        {
            // ffmpeg -i f3.mp4 -af "volumedetect" -vn -sn -dn -f null NUL
            string fbat = Constants.WorkingFolder + @"\ffmpeg.bat";
            string fout = Constants.WorkingFolder + @"\ffmpeg.txt";
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

            if (Utilities.RunCommandSync(fbat, wdir, "", 60000))
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

    }
}
