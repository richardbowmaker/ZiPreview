using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;


namespace ZiPreview
{
    public partial class FileImport : Form
    {
        public static void Run()
        {
            FileImport f = new FileImport();
            f.ShowDialog();
        }

        public FileImport()
        {
            InitializeComponent();
        }

        private void FileImport_Load(object sender, EventArgs e)
        {
            Text = Constants.Title + " Import Files";
            UpdateGui();
        }

        private void UpdateGui()
        {
            butCancel.Enabled = true;

            if (listFiles.Items.Count > 0)
            {
                butImport.Enabled = true;
                chkRemoveAudio.Enabled = true;
                chkDelete.Enabled = true;
                chkGenerateImage.Enabled = true;
            }
            else
            {
                butImport.Enabled = false;
                chkRemoveAudio.Enabled = false;
                chkDelete.Enabled = false;
                chkGenerateImage.Enabled = false;
            }
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All files (*.*)|*.*";
            dlg.Multiselect = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                listFiles.Items.AddRange(dlg.FileNames);
                UpdateGui();
            }
        }

        private bool Import(string src)
        {
            // find a drive with sufficient free space
            FileInfo fi = new FileInfo(src);
            VeracryptVolume vol = VeracryptManager
                .FindDriveWithFreeSpace(fi.Length + Constants.MinimumCaptureSpace);
            if (vol == null)
            {
                MessageBox.Show("No volumes have sufficient diskspace", Constants.Title,
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            Logger.Info("Sufficient space found on drive: " + vol.Drive);

            // destination file
            string dest = vol.Drive + Constants.FilesTargetPath + "\\" +
                Utilities.NewFilename() + Path.GetExtension(src);

            // if remove audio has been selected for a video file use ffmpeg to move the file
            // otherwise just file move or file copy the file
            string ext = Path.GetExtension(src).ToLower();
            if ((ext == ".mp4" || ext == ".avi") && chkRemoveAudio.Checked)
            {
                Logger.Info("Removing audio track from " + src);

                // ffmpeg -i file.mp4 -map 0:0 -map 0:2 -acodec copy -vcodec copy new_file.mp4 
                string cmd = Constants.FfmpegExe;
                string args = "-i \"" + src + "\" " +
                    " -map 0:0 -acodec copy -vcodec copy " +
                    "\"" + dest + "\"";
                string wdir = Path.GetDirectoryName(src);

                if (!Utilities.RunCommandSync(cmd, wdir, args, 1000000)) return false;
            }
            else
            {
                if (!Utilities.CopyFile(src, dest)) return false;
            }

            if (chkDelete.Checked)
            {
                if (!Utilities.DeleteFile(src)) return false;
            }

            // add to file set manager
            FileVolume fv = new FileVolume(dest, vol.Filename);
            FileSet fs = FileSetManager.AddFile(fv);
            if (fs == null)
            {
                Logger.Error("FileSetManager failed to add file");
                return false;
            }

            if (fs.HasVideo && chkGenerateImage.Checked)
            {
                if (!NewImage(fs)) return false;
            }

            ZipPreview.GUI.AddFileToGridTS(fs);
            return true;
        }

        private bool NewImage(FileSet file)
        {
            Logger.Info("Generating new image for " + file.VideoFilename);

            // image file name
            string image = Path.GetDirectoryName(file.VideoFilename) + "\\" +
                Path.GetFileNameWithoutExtension(file.VideoFilename) + ".jpg";

            // get duration of media
            var wmp = new WMPLib.WindowsMediaPlayer();
            var media = wmp.newMedia(file.VideoFilename);
            int duration = (int)media.duration;

            // calculate position in file, halfway or random
            int d = 0;
            Random r = new Random();
            d = r.Next(0, duration);

            // get preview image from middle
            string dss = (d / 60).ToString() + ":" + (d % 60).ToString();
            string dsf = (d / 60).ToString() + ":" + ((d + 1) % 60).ToString();

            string args = "-y -ss " + dss + " -to " + dsf + " -i " +
                file.VideoFilename + " -frames 1 " + image;

            // ffmpeg -y -ss 0:10 -to 0:11 -i <filename> -frames 1 <filename no ext>-%d.jpg
            Utilities.RunCommandSync(
                Constants.FfmpegExe,
                Constants.WorkingFolder,
                args);

            if (File.Exists(image))
            {
                file.ImageFilename = image;
                return true;
            }
            else
            {
                Logger.Error("*** Failed to create image for: " + file.VideoFilename);
                return false;
            }
        }

        private void butImport_Click(object sender, EventArgs e)
        {
            ZipPreview.GUI.GetProgressBar().Clear();
            ZipPreview.GUI.GetProgressBar().AddPart("Importing files", 1);
            ZipPreview.GUI.GetProgressBar().Start(1, listFiles.Items.Count);

            foreach (string f in listFiles.Items)
            {
                if (!Import(f))
                {
                    Logger.Error("Import aborted");
                    ZipPreview.GUI.GetProgressBar().Clear();
                    return;
                }
                ZipPreview.GUI.GetProgressBar().IncValue();
            }
            Close();
            ZipPreview.GUI.GetProgressBar().Clear();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
