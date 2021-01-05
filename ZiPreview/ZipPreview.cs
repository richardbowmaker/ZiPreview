using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace ZiPreview
{
    // thread safe if
    public interface IGuiUpdate
    {
        void AddFileToGridTS(FileSet file);
        void RefreshGridRowTS(FileSet file);
        void RefreshImageTS(FileSet file);
        void RemoveGridRowTS(FileSet file);
        void UpdateProgressTS(int value, string text);
        void SetStatusLabelTS(string text);
        void MessageBoxTS(string text, MessageBoxButtons buttons, MessageBoxIcon icon);
        void PopulateGrid();
        void SetToTopWindow();
        IntPtr GetHwnd();
        MultiPartProgressBar GetProgressBar();
    }
    
    public partial class ZipPreview : Form, IGuiUpdate, IImagesViewerData, IEasyMenu
    {
        private ImagesViewer _images;
        private CPicture _image;
        private Panel _imagesPanel;
        private Panel _imagePanel;
        private int _noOfImages;
        private VideoCapture _videoCapture;
        private bool _initialised = false;
        private Random _rnd;
        private MultiPartProgressBar _progressBar;

        public ZipPreview()
        {
            InitializeComponent();
        }

        private void ZiPreview_Load(object sender, EventArgs e)
        {
            string cd = Directory.GetCurrentDirectory();

            Logger.TheListBox = listTrace;
            Logger.Level = Logger.LoggerLevel.Info;

            _rnd = new Random();

            if (Debugger.IsAttached)
            {
                DirectoryInfo di = Directory.GetParent(cd);
                di = Directory.GetParent(di.FullName);
                di = Directory.GetParent(di.FullName);
                Constants.WorkingFolder = di.FullName;
            }
            else
            {
                DirectoryInfo di = Directory.GetParent(cd);
                Constants.WorkingFolder = di.FullName;
            }
            Logger.Info("Working folder = " + Constants.WorkingFolder);

            KeyPreview = true;
            Text = Constants.Title;
            _noOfImages = 3;
            statusProgress.Minimum = 0;
            statusProgress.Maximum = statusProgress.Width;
            statusProgress.Value = 0;
            _progressBar = new MultiPartProgressBar(statusProgress.Width);
            statusLabel.Text = "";

            splitHorizGridTrace.SplitterDistance = 500;

            // prep the video capture
            _videoCapture = new VideoCapture();
            if (!_videoCapture.Initialise())
            {
                Close();
                return;
            }

            // create two panels and add to vertical split RHS
            _imagesPanel = new Panel();
            splitVertical.Panel2.Controls.Add(_imagesPanel);
            _imagesPanel.Dock = DockStyle.Fill;

            _imagePanel = new Panel();
            splitVertical.Panel2.Controls.Add(_imagePanel);
            _imagePanel.Dock = DockStyle.Fill;

            // hide the image panel
            _imagePanel.Hide();

            // display the images panel
            _images = new ImagesViewer(_imagesPanel, this);
            _images.Initialise();
            _imagesPanel.Show();

            // hide the image viewer
            _image = new CPicture(_imagePanel);
            _imagePanel.Hide();

            // set thread safe callback
            GUI = this;

            InitialiseGrid();

            _initialised = true;

            EasyMenu.SetCallback(this);
        }

        private void Timer__Tick(object sender, EventArgs e)
        {
            CaptureClipboard.TimerEvent();
        }

        private void FrmZiPreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CanClose();
        }

        #region IImagesViewerData
        //-----------------------------------------------------
        // IImagesViewerData
        //-----------------------------------------------------
        public int GetNoOfImages()
        {
            return gridFiles.Rows.Count;
        }
        public int GetNoOfRows()
        {
            return _noOfImages;
        }
        public int GetNoOfCols()
        {
            return _noOfImages;
        }
        public string GetThumbNail(int n)
        {
            return ((FileSet)gridFiles.Rows[n].Tag).ImageFilename;
        }
        public string GetVideo(int n)
        {
            return ((FileSet)gridFiles.Rows[n].Tag).VideoFilename;
        }
        public void ImageSelected(int n)
        {
            gridFiles.Rows[n].Selected = true;
        }
        public void ImagePlay(int n)
        {
            PlayFile((FileSet)gridFiles.Rows[n].Tag);
        }
        #endregion

        #region IGuiUpdate
        //-----------------------------------------------------
        // IGuiUpdate
        //-----------------------------------------------------

        // global reference to GUI interface
        public static IGuiUpdate GUI { set; get; }

        private delegate void VoidFileT(FileSet file);
        private delegate void VoidString(string str);
        private delegate void VoidDataGridViewRow(DataGridViewRow row);
        private delegate void VoidVoid();
        private delegate void VoidIntInt(int int1, int int2);
        private delegate void VoidIntString(int int1, string s);
        private delegate void VoidStringMessageBoxButtonsAndIcon(string text, MessageBoxButtons buttons, MessageBoxIcon icon);

        public void RefreshGridRowTS(FileSet file)
        {
            if (InvokeRequired)
            {
                VoidFileT action = new VoidFileT(RefreshGridRowTS);
                Invoke(action, new object[] { file });
            }
            else
            {
                gridFiles.Refresh();
            }
        }

        public void RefreshImageTS(FileSet file)
        {
            if (InvokeRequired)
            {
                VoidFileT action = new VoidFileT(RefreshImageTS);
                Invoke(action, new object[] { file });
            }
            else
            {
                _images.RefreshImage(file.Row.Index);
            }
        }

        public void AddFileToGridTS(FileSet file)
        {
            if (InvokeRequired)
            {
                VoidFileT action = new VoidFileT(AddFileToGridTS);
                Invoke(action, new object[] { file });
            }
            else
            {
                // add to grid
                int r = gridFiles.Rows.Add();
                DataGridViewRow row = gridFiles.Rows[r];

                // have the row and FileT object reference each other, GC can cope 
                row.Tag = (object)file;
                file.Row = row;
            }
        }

        public void RemoveGridRowTS(FileSet file)
        {
            if (InvokeRequired)
            {
                VoidFileT action = new VoidFileT(RemoveGridRowTS);
                Invoke(action, new object[] { file });
            }
            else
            {
                // add to grid
                gridFiles.Rows.RemoveAt(file.Row.Index);
            }
        }

        public void UpdateProgressTS(int value, string text)
        {
            if (InvokeRequired)
            {
                VoidIntString action = new VoidIntString(UpdateProgressTS);
                Invoke(action, new object[] { value, text });
            }
            else
            {
                statusProgress.Value = value;
                statusLabel.Text = text;
                statusStrip.Refresh();
            }
        }

        public void SetStatusLabelTS(string text)
        {
            if (InvokeRequired)
            {
                VoidString action = new VoidString(SetStatusLabelTS);
                Invoke(action, new object[] { text });
            }
            else
            {
                statusLabel.Text = text;
            }
        }

        public void MessageBoxTS(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (InvokeRequired)
            {
                VoidStringMessageBoxButtonsAndIcon action = new VoidStringMessageBoxButtonsAndIcon(MessageBoxTS);
                Invoke(action, new object[] { text, buttons, icon});
            }
            else
            {
                MessageBox.Show(text, Constants.Title, buttons, icon);
            }
        }

        public void PopulateGrid()
        {
            PopulateGrid_();
        }

        public void SetToTopWindow()
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Maximized;
            else
            {
                TopMost = true;
                Focus();
                BringToFront();
                TopMost = false;
            }
        }

        public IntPtr GetHwnd()
        {
            return Handle;
        }

        public MultiPartProgressBar GetProgressBar()
        {
            return _progressBar;
        }

        private void GridFiles_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (gridFiles.Rows.Count == 0) return;

            string col = gridFiles.Columns[e.ColumnIndex].Name;
            FileSetManager.SortFieldT f = FileSetManager.SortFieldT.Filename;

            switch (e.ColumnIndex)
            {
                case 0: f = FileSetManager.SortFieldT.Filename; break;
                case 1: f = FileSetManager.SortFieldT.Selected; break;
                case 2: f = FileSetManager.SortFieldT.Type; break;
                case 3: f = FileSetManager.SortFieldT.LastDate; break;
                case 4: f = FileSetManager.SortFieldT.Times; break;
            }

            FileSetManager.SortFiles(f);

            if (FileSetManager.GetFiles().Count == gridFiles.RowCount)
            {
                for (int i = 0; i < gridFiles.RowCount; ++i)
                {
                    FileSet fs = FileSetManager.GetFiles()[i];
                    DataGridViewRow row = gridFiles.Rows[i];
                    row.Tag = fs;
                    fs.Row = row;
                }
                gridFiles.Refresh();
                gridFiles.FirstDisplayedScrollingRowIndex = 0;
                _images.StopPlay();
                _images.Draw(0);
                _images.SetSelected(0);
            }
            else Logger.Error("Oops !, program bug"); // shouldn't happen
        }

        #endregion

        #region Grid Handling

        private void InitialiseGrid()
        {
            // grid initialise
            gridFiles.RowHeadersVisible = false;
            gridFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridFiles.MultiSelect = false;
            gridFiles.Rows.Clear();
            gridFiles.VirtualMode = true;
            gridFiles.AllowUserToAddRows = false;
            gridFiles.AllowUserToDeleteRows = false;

            DataGridViewTextBoxColumn col;
            col = new DataGridViewTextBoxColumn()
            {
                Width = 170,
                HeaderText = "Filename",
                Name = "colFilename"
            };
            gridFiles.Columns.Add(col);
            col = new DataGridViewTextBoxColumn()
            {
                Width = 15,
                HeaderText = "",
                Name = "colSelected"
            };
            gridFiles.Columns.Add(col);
            col = new DataGridViewTextBoxColumn()
            {
                Width = 40,
                HeaderText = "Type",
                Name = "colType"
            };
            gridFiles.Columns.Add(col);
            col = new DataGridViewTextBoxColumn()
            {
                Width = 80,
                HeaderText = "Date",
                Name = "colDate"
            };
            gridFiles.Columns.Add(col);
            col = new DataGridViewTextBoxColumn()
            {
                Width = 40,
                HeaderText = "Times",
                Name = "colTimes"
            };
            gridFiles.Columns.Add(col);
            col = new DataGridViewTextBoxColumn()
            {
                Width = 70,
                HeaderText = "Volume",
                Name = "colVolume"
            };
            gridFiles.Columns.Add(col);
            col = new DataGridViewTextBoxColumn()
            {
                Width = 60,
                HeaderText = "Duration",
                Name = "colDuration"
            };
            gridFiles.Columns.Add(col);

        }

        public void PopulateGrid_()
        {
            _images.StopPlay();
            Cursor.Current = Cursors.WaitCursor;

            // populate th grid
            FileSetManager.PopulateFiles(VeracryptManager.Volumes);
            gridFiles.Rows.Clear();
            LoadGrid();
            _images.Draw(0);
            Logger.Info("Files: " + gridFiles.Rows.Count.ToString());
            FileSetManager.CreateImages();

            // generate unmount script
            List<string> script = VeracryptManager.GenerateUnmountScript();
            StreamWriter sw = new StreamWriter(Constants.UnmountFile);
            script.ForEach(s => sw.WriteLine(s));
            sw.Close();

            Cursor.Current = Cursors.Default;
        }

        private void LoadGrid()
        {
            _images.Initialise();
            List<FileSet> files = FileSetManager.GetFiles();

 //           UpdateProgressTS(0, files.Count);
            foreach (FileSet file in files)
            {
                AddFileToGridTS(file);
 //               IncrementProgressTS();
            }

            gridFiles.Refresh();
//            ClearProgressTS();
        }

        private void GridFiles_SelectionChanged(object sender, EventArgs e)
        {
            SelectImagesViewer();
            DataGridViewSelectedRowCollection rows = gridFiles.SelectedRows;
            if (rows.Count == 1)
            {
                DataGridViewRow r = rows[0];
                FileSet file = (FileSet)r.Tag;

                if (file != null)
                {
                    _images.SetSelected(r.Index);

                    int fri = gridFiles.FirstDisplayedScrollingRowIndex;
                    int lri = fri + gridFiles.DisplayedRowCount(false) - 1;

                    if (r.Index < fri || r.Index > lri)
                    {
                        gridFiles.FirstDisplayedScrollingRowIndex = r.Index;
                    }

                    Text = Constants.Title + " - " + file.FilenameNoPathAndExt;
                }
            }
        }

        private void GridFiles_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow row = gridFiles.Rows[e.RowIndex];
                FileSet file = (FileSet)row.Tag;

                List<string> tt = new List<string>();

                if (file.HasImage) tt.Add("Image: " + file.ImageFilename);
                if (file.HasVideo) tt.Add("Video: " + file.VideoFilename);
                if (file.HasLink)
                {
                    StreamReader sr = new StreamReader(file.LinkFilename);
                    string l = sr.ReadLine();
                    sr.Close();
                    tt.Add("Link: " + l);
                }

                if (file.HasVideo)
                {
                    long fs = new FileInfo(file.VideoFilename).Length;
                    tt.Add("File size: " + String.Format("{0:n0}", fs));
                }

                tt.Add("Volume: " + file.Volume);

                foreach (string s in tt)
                {
                    e.ToolTipText +=
                        (e.ToolTipText.Length == 0 ? "" : "\n") + s;
                }
            }
        }

        private void GridFiles_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridViewRow row = gridFiles.Rows[e.RowIndex];
                FileSet file = (FileSet)row.Tag;

                switch (e.ColumnIndex)
                {
                    case 0: e.Value = file.ShortFilename; break;
                    case 1: e.Value = file.SelectedS; break;
                    case 2: e.Value = file.TypeS; break;
                    case 3: e.Value = file.LastDate; break;
                    case 4: e.Value = file.Times; break;
                    case 5: e.Value = file.VolumeDb; break;
                    case 6: e.Value = file.Duration; break;
                }
            }
        }


        #endregion

        #region Key Mapping

        // easy menu key handling
        private bool keyDown_;
        private long keyDownTime_;

        public int GetNoOfOptions()
        {
            return 5;
        }
        public string GetOptionText(int n)
        {
            switch (n)
            {
                case 0: return "Play video";
                case 1: return "Copy link to clipboard";
                case 2: return "Launch browser";
                case 3: return "Random preview";
                case 4: return "Link to browser";
                default: return "";
            }
        }

        public bool GetOptionEnabled(int n)
        {
            var rows = gridFiles.SelectedRows;
            if (rows.Count == 1)
            {
                FileSet file = (FileSet)rows[0].Tag;
                switch (n)
                {
                    case 0: return file.HasVideo;
                    case 1: return file.HasLink;
                    case 2: return file.HasLink;
                    case 4: return file.HasLink;
                    default: return true;
                }
            }
            else
                return false;
        }
        public void OptionSelected(int n)
        {
            var rows = gridFiles.SelectedRows;
            if (rows.Count == 1)
            {
                FileSet file = (FileSet)rows[0].Tag;

                switch (n)
                {
                    case 0: if (file != null) PlayFile(file); break;
                    case 1: if (file != null) CopyLinkToClipboard(file); break;
                    case 2: if (file != null) Utilities.LaunchBrowser(file); break;
                    case 3: RandomPage(); break;
                    case 4: if (file != null) GotoBrowserLink(file); break;
                }
            }
        }

        private void ZipPreview_KeyDown(object sender, KeyEventArgs e)
        {
            if (!keyDown_)
            {
                keyDown_ = true;
                keyDownTime_ = DateTime.Now.Ticks;
            }

            if (e.KeyData != Keys.Space)
                if (HandleKey(e.KeyData)) e.Handled = true;
        }

        private void ZipPreview_KeyUp(object sender, KeyEventArgs e)
        {
            keyDown_ = false;

            long dt = DateTime.Now.Ticks - keyDownTime_;
            if (e.KeyData == Keys.Space && dt > 5000000)
                EasyMenu.Run();
 
        }

        public bool HandleKey(Keys KeyData)
        {
            var rows = gridFiles.SelectedRows;
            if (rows.Count == 1)
            {
                FileSet file = (FileSet)rows[0].Tag;
                if (file != null)
                {
                    switch (KeyData)
                    {
                        case Keys.Add: ToggleSelect(file); return true;
                        case Keys.Enter: PlayFile(file); return true;
                        case Keys.Up: Up(file); return true;
                        case Keys.Down: Down(file); return true;
                        case Keys.PageDown: PageDown(file); return true;
                        case Keys.PageUp: PageUp(file); return true;
                        case Keys.Escape: SelectImagesViewer(); return true;

                        case Keys.Right:
                        case Keys.Space:
                            NextFile(file); return true;

                        case Keys.Left:
                            PreviousFile(file); return true;

                        case Keys.Z:
                        case Keys.V:
                        case Keys.X:
                        case Keys.C:
                            PageDown(file); return true;

                        case Keys.Q:
                        case Keys.W:
                        case Keys.E:
                        case Keys.R:
                            PageUp(file); return true;

                        case Keys.F1: NextSelectedFile(file); return true;
                        case Keys.F2: PreviousSelectedFile(file); return true;
                        case Keys.F3: RandomPage(); return true;
                        case Keys.F4: Utilities.LaunchBrowser(file); return true;
                        case Keys.F5: IncNoOfImages(file); return true;
                        case Keys.F6: DecNoOfImages(file); return true;
                        case Keys.F7: CaptureVideo(file); return true;
                        case Keys.F11: TogglePreview(); return true;
                        case Keys.F12: CopyLinkToClipboard(file); return true;

                        case Keys.Delete: DeleteSelected(); return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region Functions
        private bool CanClose()
        {
            if (!_initialised) return true;

            _images.StopPlay();

            // is a capture in progress
            if (_videoCapture.IsCaptureInProgress)
            {
                DialogResult res = MessageBox.Show("Video capture in progress, continue to exit ?",
                Constants.Title,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

                if (res == DialogResult.Cancel) return false;

                _videoCapture.StopCapture();
            }

            if (VeracryptManager.HasMountedVolumes)
            {
                FileSetManager.WriteProperties();

                DialogResult res = MessageBox.Show("Do you want to unmount the volumes ?",
                Constants.Title,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button3);

                switch (res)
                {
                    case DialogResult.Cancel: return false;
                    case DialogResult.Yes: Dismount(); break;
                }
            }

            _images.Uninitialise();
            _videoCapture.Uninitialise();
            _image.Uninitialise();
            Clipboard.Clear();
            return true;
        }

        private void DeleteSelected()
        {
            DataGridViewSelectedRowCollection rs = gridFiles.SelectedRows;
            if (rs.Count == 1)
            {
                _images.StopPlay();
                VerifyDelete.Run((FileSet)rs[0].Tag);
                _images.Refresh();
            }
        }

        private void IncNoOfImages(FileSet file)
        {
            if (_noOfImages < 5)
            {
                _images.Uninitialise();
                _noOfImages++;
                _images.Initialise();
                _images.Draw(file.Row.Index);
                _images.SetSelected(file.Row.Index);
            }
        }
        private void DecNoOfImages(FileSet file)
        {
            if (_noOfImages > 1)
            {
                _images.Uninitialise();
                _noOfImages--;
                _images.Initialise();
                _images.Draw(file.Row.Index);
                _images.SetSelected(file.Row.Index);
            }
        }

        private void NextSelectedFile(FileSet file)
        {
            int r = file.Row.Index;
            int nr = r;

            while ((nr = ((nr + 1) % gridFiles.Rows.Count)) != r)
            {
                if (((FileSet)gridFiles.Rows[nr].Tag).Selected)
                {
                    gridFiles.Rows[nr].Selected = true;
                    _images.SetSelected(nr);
                    break;
                }
            }
        }
        private void PreviousSelectedFile(FileSet file)
        {
            int r = file.Row.Index;
            int nr = r;

            while ((nr = ((nr -1 + gridFiles.Rows.Count) % gridFiles.Rows.Count)) != r)
            {
                if (((FileSet)gridFiles.Rows[nr].Tag).Selected)
                {
                    gridFiles.Rows[nr].Selected = true;
                    _images.SetSelected(nr);
                    break;
                }
            }
        }

        private void SelectImagesViewer()
        {
            _imagePanel.Hide();
            _imagesPanel.Show();
        }

        private void Up(FileSet file)
        {
            int n = (file.Row.Index + gridFiles.Rows.Count - GetNoOfCols()) % gridFiles.Rows.Count;
            gridFiles.Rows[n].Selected = true;
        }

        private void Down(FileSet file)
        {
            int n = (file.Row.Index + GetNoOfCols()) % gridFiles.Rows.Count;
            gridFiles.Rows[n].Selected = true;
        }

        private void PageUp(FileSet file)
        {
            int n = (_images.Top + gridFiles.Rows.Count - (GetNoOfRows() * GetNoOfCols())) % gridFiles.Rows.Count;
            gridFiles.Rows[n].Selected = true;
        }
        private void PageDown(FileSet file)
        {
            int n = (_images.Top + (GetNoOfRows() * GetNoOfCols())) % gridFiles.Rows.Count;
            gridFiles.Rows[n].Selected = true;
        }

        private void PreviousFile(FileSet file)
        {
            int n = (file.Row.Index + gridFiles.Rows.Count - 1) % gridFiles.Rows.Count;
            gridFiles.Rows[n].Selected = true;
        }

        private void NextFile(FileSet file)
        {
            int n = (file.Row.Index + gridFiles.Rows.Count + 1) % gridFiles.Rows.Count;
            gridFiles.Rows[n].Selected = true;
        }

        private void ToggleSelect(FileSet file)
        {
            file.ToggleSelected();
            file.Row.Cells["colSelected"].Value = file.SelectedS;
            RefreshGridRowTS(file);
        }

        private void CopyLinkToClipboard(FileSet file)
        {
            if (file.HasLink)
                Clipboard.SetText(file.Link, TextDataFormat.Text);
        }
        private void CopyFilenameToClipboard(FileSet file)
        {
            if (file.Filename.Length > 0)
                Clipboard.SetText(file.Filename, TextDataFormat.Text);
        }

        private void CaptureVideo(FileSet file)
        {
            // file must have a link
            if (!file.HasLink)
            {
                MessageBox.Show("File does not have a link", Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // warning if file already has video
            if (file.HasVideo)
            {
                DialogResult dr = MessageBox.Show("File already has video, do you want to overwrite it", 
                    Constants.Title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                if (dr != DialogResult.Yes) return;

                // free the space
                Utilities.DeleteFile(file.VideoFilename);
                file.VideoFilename = "";
                RefreshGridRowTS(file);
            }
            _videoCapture.StartCapture(file);
        }

        public void PlayFile(FileSet file)
        {
            if (file.HasVideo)
            {
                _images.StopPlay();

                file.IncCount("times");
                file.SetDateStamp("lasttime");
                RefreshGridRowTS(file);

                // set volume level
                Utilities.SetAudioLevel(50);

                ProcessStartInfo psi = new ProcessStartInfo(file.VideoFilename);
                psi.UseShellExecute = true;
                psi.WindowStyle = ProcessWindowStyle.Maximized;
                Process.Start(psi);
            }
            else if (file.HasLink)
            {
                file.IncCount("times");
                file.SetDateStamp("lasttime");
                RefreshGridRowTS(file);

                Utilities.LaunchBrowser(file);
            }
            else if (file.HasImage)
            {
                _imagesPanel.Hide();
                _imagePanel.Show();

                // hide the images preview and show the image full size
                _image.LoadFile(file.ImageFilename);

                file.IncCount("times");
                file.SetDateStamp("lasttime");
                RefreshGridRowTS(file);
            }
        }

        private void GotoLink(FileSet file)
        {
            Utilities.LaunchBrowser(file);
        }

        private void RandomPage()
        {
            if (gridFiles.Rows.Count == 0) return;
            int r = _rnd.Next(0, gridFiles.Rows.Count - 1);
            gridFiles.FirstDisplayedScrollingRowIndex = r;
            gridFiles.Rows[r].Selected = true;
            _images.Draw(r);
            _images.SetSelected(r);
        }

        private void Dismount()
        {
            FileSetManager.WriteProperties();
            VeracryptManager.UnmountVolumes();
        }

        private void ClearSelected()
        {
            FileSetManager.GetFiles().ForEach(f => f.Selected = false);
            gridFiles.Refresh();
        }

        private void TogglePreview()
        {
            _images.PreviewOn = !_images.PreviewOn;
        }

        private void ImportFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
                FileSetManager.New(dlg.FileName);
        }

        private void ProcessVideo(FileSet file)
        {
            if (file.HasVideo)
                VideoAnalyser.Run(file);
            else
                MessageBox.Show("No video file", Constants.Title, 
                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        // places the link in the clipboard and sets focus to the browser window
        private void GotoBrowserLink(FileSet file)
        {
            if (file.HasLink)
            {
                Clipboard.SetText(file.Link, TextDataFormat.Text);
                IntPtr w = Utilities.FindWindow("Tor Browser");
                if (w != (IntPtr)0) Utilities.SetActiveWindow(w);
            }
        }

        #endregion

        #region Menu Handlers
        private void FileSelectMenu_Click(object sender, EventArgs e)
        {
            if (!SelectDrives.Run())
            {
                MessageBox.Show("Error whilst mounting volumes, program closing", Constants.Title,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void FileDeleteMenu_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        private void FileClearSelectedMenu_Click(object sender, EventArgs e)
        {
            ClearSelected();
        }

        private void FileImportMenu_Click(object sender, EventArgs e)
        {
            ImportFile();
        }

        private void FileExitMenu_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ViewMenu_DropDownOpening(object sender, EventArgs e)
        {
            if (_images.PreviewOn)
                viewPreviewMenu.Text = "Preview off (F11)";
            else
                viewPreviewMenu.Text = "Preview on (F11)";
        }

        private void ViewMoreImagesMenu_Click(object sender, EventArgs e)
        {
            var rows = gridFiles.SelectedRows;
            if (rows.Count == 1) IncNoOfImages((FileSet)rows[0].Tag);
        }

        private void ViewLessImagesMenu_Click(object sender, EventArgs e)
        {
            var rows = gridFiles.SelectedRows;
            if (rows.Count == 1) DecNoOfImages((FileSet)rows[0].Tag);
        }

        private void ViewViewMenu_Click(object sender, EventArgs e)
        {
            var rows = gridFiles.SelectedRows;
            if (rows.Count == 1) PlayFile((FileSet)rows[0].Tag);
        }

        private void ViewNextSelectedMenu_Click(object sender, EventArgs e)
        {
            var rows = gridFiles.SelectedRows;
            if (rows.Count == 1) NextSelectedFile((FileSet)rows[0].Tag);
        }

        private void ViewPreviousSelectedMenu_Click(object sender, EventArgs e)
        {
            var rows = gridFiles.SelectedRows;
            if (rows.Count == 1) PreviousSelectedFile((FileSet)rows[0].Tag);
        }
        private void ViewLinkMenu_Click(object sender, EventArgs e)
        {
            var rows = gridFiles.SelectedRows;
            if (rows.Count == 1) GotoLink((FileSet)rows[0].Tag);
        }

        private void ViewRandomPageMenu_Click(object sender, EventArgs e)
        {
            RandomPage();
        }

        private void ViewLinkToClipboardMenu_Click(object sender, EventArgs e)
        {
            var rows = gridFiles.SelectedRows;
            if (rows.Count == 1) CopyLinkToClipboard((FileSet)rows[0].Tag);
        }

        private void ViewFilenameToClipboardMenu_Click(object sender, EventArgs e)
        {
            var rows = gridFiles.SelectedRows;
            if (rows.Count == 1) CopyFilenameToClipboard((FileSet)rows[0].Tag);
        }

        private void ViewPreviewMenu_Click(object sender, EventArgs e)
        {
            TogglePreview();
        }

        private void ToolsMenu_DropDownOpening(object sender, EventArgs e)
        {
            if (CopyFilesThread.IsRunning)
                toolsCopyFilesMenu.Text = "Abort copy files";
            else
                toolsCopyFilesMenu.Text = "Copy files ...";

            if (_videoCapture.IsCaptureInProgress)
                toolsCaptureMenu.Text = "Abort capture";
            else
                toolsCaptureMenu.Text = "Capture video ...";
        }


        private void ToolsCaptureMenu_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rs = gridFiles.SelectedRows;
            if (rs.Count == 1)
            {
                _images.StopPlay();
                CaptureVideo((FileSet)rs[0].Tag);
            }
        }

        private void ToolsCopyFilesMenu_Click(object sender, EventArgs e)
        {
            if (CopyFilesThread.IsRunning)
                CopyFilesThread.Stop();
            else
                CopyFiles.Show_();
        }

        private void ToolsSaveLogMenu_Click(object sender, EventArgs e)
        {
            Logger.WriteToFile();
        }

        private void ToolsClearLogMenu_Click(object sender, EventArgs e)
        {
            Logger.Clear();
        }

        private void ToolsVolumePropertiesMenu_Click(object sender, EventArgs e)
        {
            VolumeProperties.Run();
        }

        private void ToolsRunTestsMenu_Click(object sender, EventArgs e)
        {
            FileSet.PropertyTests();
        }

        private void ToolsLaunchBrowserMenu_Click(object sender, EventArgs e)
        {
            Utilities.LaunchBrowser();
        }

        private void ToolsProcessVideoMenu_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rs = gridFiles.SelectedRows;
            if (rs.Count == 1)
            {
                ProcessVideo((FileSet)rs[0].Tag);
            }
        }

        private void ToolsTestHarnessMenu_Click(object sender, EventArgs e)
        {
            MultiPartProgressBar.Test();          
        }

        #endregion

        #region Context Menu Handlers

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void loadBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        #endregion
    }
}
