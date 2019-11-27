using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace ZiPreview
{
    public partial class CopyFiles : Form
    {
        private FolderBrowserDialog _folderSelectDlg;

        public static void Show_()
        {
            CopyFiles frm = new CopyFiles();
            frm.ShowDialog();
        }

        public CopyFiles()
        {
            InitializeComponent();
        }


        private void CopyFiles_Load(object sender, EventArgs e)
        {
            Text = Constants.Title + " Copy Files";
            _folderSelectDlg = new FolderBrowserDialog();
            MaximumSize = Size;
            MinimumSize = Size;
            List<DriveVolume> drives = VeracryptManager.GetDrives();
            drives.ForEach(v => checkedListBoxDestFolders.Items.Add(v.Drive + "  " + v.Volume, false));
        }

        private void ButAddSourceFolder_Click(object sender, EventArgs e)
        {
            DialogResult dr = _folderSelectDlg.ShowDialog();
         
            if (dr == DialogResult.OK)
            {
                string fdr = _folderSelectDlg.SelectedPath;

                if (fdr.Length < 4)
                {
                    MessageBox.Show("Must select a folder, not just a drive", Constants.Title + ", copy files",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                listBoxSourceFolders.Items.Add(fdr);
            }
        }

        private void CopyFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            _folderSelectDlg = null;
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButCopy_Click(object sender, EventArgs e)
        {
            butCopy.Enabled = false;
            butCancel.Enabled = false;

            List<string> srcFolders = new List<string>();
            List<string> destFolders = new List<string>();

            for (int i = 0; i < listBoxSourceFolders.Items.Count; ++i)
            {
                srcFolders.Add(listBoxSourceFolders.Items[i].ToString());
            }

            // make list of selected destination drives
            for (int i = 0; i < checkedListBoxDestFolders.Items.Count; ++i)
            {
                if (checkedListBoxDestFolders.GetItemChecked(i))
                {
                    string dr = checkedListBoxDestFolders.Items[i].ToString();
                    int n = dr.IndexOf(' ');
                    if (n != -1) dr = dr.Substring(0, n);
                    destFolders.Add(dr);
                }
            }

            CopyFilesThread.CopyFiles(srcFolders, destFolders);
            Close();
        }
    }

    public class CopyFilesThread
    {

        private static int _totalFiles;
        private static int _filesCopied;
        private static int _filesExisting;
        private static long _bytesCopied;
        private static List<string> _srcFolders;
        private static List<string> _destFolders;
        private static int _destFolderIx;

        private static Thread _thread = null;

        enum StateT { Stopped, Running, StopRequest };
        private static long _state = (long)StateT.Stopped;

        public CopyFilesThread()
        {
        }

        public static bool CopyFiles(List<string> srcFolders, List<string> destFolders)
        {
            if (Interlocked.Read(ref _state) != (long)StateT.Stopped) return false;

            // scan source folders to build list of files to copy
            // sort them so that they appear in groups

            // construct group of files with same name aprt from extension
            // and calculate total size

            // based on path less drive check to see if it exists in any
            // of the destination folders, if so don't copy

            // if current destination folder has sufficient space than
            // copy group to destination drive, with same filepath

            // if not enough space move to next destination etc.

            // if run out of space stop copy

            _destFolderIx = 0;
            _totalFiles = 0;
            _filesCopied = 0;
            _filesExisting = 0;
            _bytesCopied = 0;

            _srcFolders = srcFolders;
            _destFolders = destFolders;

            Logger.Info("Copying all files from:");
            foreach (string s in _srcFolders) Logger.Info(s);
            Logger.Info("to:");
            foreach (string s in _destFolders) Logger.Info(s);

            _thread = new Thread(() => RunCopyFiles());
            _thread.Start();

            return true;
        }

        private static void Finish()
        {
            OutputSummary();
            _destFolders = null;
            _srcFolders = null;
            _thread = null;

            ZipPreview.ZiPreview.UpdateProgressTS(0, 0);
            ZipPreview.ZiPreview.SetStatusLabelTS("");
        }

        public static bool Stop()
        {
            // signal thread to stop
            if (Interlocked.CompareExchange(ref _state,
                (long)StateT.StopRequest, (long)StateT.Running) != (long)StateT.Running)
            {
                Logger.Error("Thread not running");
                return false;
            }
            return true;
        }

        public static bool IsRunning
        {
            get { return Interlocked.Read(ref _state) == (long)StateT.Running; }
        }

        private static bool CheckStop()
        {
            if (Interlocked.CompareExchange(ref _state, 
                (long)StateT.Stopped, (long)StateT.StopRequest) == (long)StateT.StopRequest)
            {
                Finish();
                Logger.Error("*** Copy aborted ***");
                return true;
            }
            return false;
        }

        private static void UpdateProgress()
        {
            ZipPreview.ZiPreview.UpdateProgressTS(_filesCopied + _filesExisting, _totalFiles);
            ZipPreview.ZiPreview.SetStatusLabelTS(
                String.Format("{0:#,##0} of {1:#,##0} files processed, {2:#,##0} bytes", 
                    _filesCopied + _filesExisting, _totalFiles, _bytesCopied));
        }

        private static void RunCopyFiles()
        {
            Interlocked.Exchange(ref _state, (long)StateT.Running);

            List<string> srcFiles = new List<string>();

            foreach (string folder in _srcFolders)
            {
                string[] files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
                srcFiles.AddRange(files);

                if (CheckStop()) return;
            }
            _totalFiles = srcFiles.Count;

            // quit if no source files or destination drives
            if (_destFolders.Count == 0 || _totalFiles == 0)
            {
                OutputSummary();
                Interlocked.CompareExchange(ref _state, 
                    (long)StateT.Stopped, (long)StateT.Running);
                return;
            }

            UpdateProgress();

            // organise files into groups that hasve the same
            // file path except for the extension
            srcFiles.Sort();
            List<string> group = new List<string>();
            List<List<string>> groups = new List<List<string>>();
            string stem = "";

            foreach (string file in srcFiles)
            {
                if (stem.CompareTo(GetFilePathNoExt(file)) != 0)
                {
                    if (group.Count > 0) groups.Add(group);
                    group = new List<string>();
                }
                group.Add(file);
                stem = GetFilePathNoExt(file);

                if (CheckStop()) return;
            }
            if (group.Count > 0) groups.Add(group);

            // copy files in groups 
            foreach (List<string> grp in groups)
            {
                if (!FileGroupAlreadyExists(grp))
                {
                    CopyFileGroup(grp);
                    //if (!CopyFileGroup(grp))
                    //{
                    //    OutputSummary();
                    //    Interlocked.CompareExchange(ref _state,
                    //        (long)StateT.Stopped, (long)StateT.Running);
                    //    return;
                    //}
                }
                UpdateProgress();
                if (CheckStop()) return;
            }

            Finish();
            Interlocked.CompareExchange(ref _state, 
                (long)StateT.Stopped, (long)StateT.Running);
        }

        private static void OutputSummary()
        {
            Logger.Info("Total files: " + _totalFiles.ToString());
            Logger.Info("Files copied: " + _filesCopied.ToString() + " (" + _bytesCopied + " bytes)");
            Logger.Info("Files already existing: " + _filesExisting.ToString());
            Logger.Info("Files not copied: " + (_totalFiles - _filesCopied).ToString());
        }

        private static bool FileGroupAlreadyExists(List<string> files)
        {
            if (files.Count == 0) return false;

            string fnd = GetFilePathNoDrive(files[0]);
            string drive = "";

            foreach (string destFolder in _destFolders)
            {
                drive = destFolder;
                if (File.Exists(destFolder + "\\" + fnd))
                {
                    // first file found in a destination drive
                    // check that all the others exists there too
                    int n = 1;
                    _filesExisting++;
                    while (n < files.Count)
                    {
                        fnd = GetFilePathNoDrive(files[n]);
                        if (!File.Exists(destFolder + "\\" + fnd)) return false;
                        ++n;
                        _filesExisting++;
                    }

                    if (files.Count == n)
                    {
                        // all files found in destination
                        Logger.Error("File(s) already exist in: " + drive);
                        foreach (string file in files) Logger.Error("    " + file);
                        return true;
                    }
                }
            }

            return false;
        }
        private static bool CopyFileGroup(List<string> group)
        {
            _destFolderIx = 0;

            // get total size of files
            long fsize = 0;
            foreach (string file in group)
            {
                FileInfo fi = new FileInfo(file);
                fsize += fi.Length + 2000; // disk space size will be higher
            }

            // find disk that has available space
            string destFolder;
            long? free;

            while (true)
            {
                destFolder = _destFolders[_destFolderIx];
                free = Utilities.GetDriveFreeSpace(destFolder);

                if (free.HasValue && fsize < free) break;
                if (++_destFolderIx >= _destFolders.Count)
                {
                    Logger.Error("Run out of space on destination drives, files not copied:");
                    foreach (string s in group) Logger.Error(s);
                    return false;
                }
            }

            // copy files to the group
            foreach (string file in group)
            {
                // create destination directory
                string dest = destFolder + "\\" + GetFilePathNoDrive(file);
                string dpath = Path.GetDirectoryName(dest);
                if (!Directory.Exists(dpath))
                {
                    Directory.CreateDirectory(dpath);
                    Logger.Info("Created output directory: " + dpath);
                }

                // copy file only if destination does not exist
                if (!File.Exists(dest) &&
                     VeracryptManager.IsMountedVolume(file) &&
                     VeracryptManager.IsMountedVolume(dest))
                {
                    Utilities.CopyFile(file, dest);
                    Logger.Info("Copied " + file + " to " + dest);
                    _filesCopied++;
                    FileInfo fi = new FileInfo(dest);
                    _bytesCopied += fi.Length;
                }
                else
                {
                    Logger.Error("Destination invalid: " + dest);
                    _filesExisting++;
                }
            }
            return true;
        }

        private static string GetFilePathNoExt(string path)
        {
            string f = Path.GetDirectoryName(path) + "\\" +
                Path.GetFileNameWithoutExtension(path);
            if (f.EndsWith("-1"))
            {
                f = f.Substring(0, f.Length - 2);
            }
            return f;
        }

        private static string GetFilePathNoDrive(string path)
        {
            return path.Substring(3);
        }

    }
}
