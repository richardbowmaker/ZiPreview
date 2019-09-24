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
        void AddFileToGridTS(FileT file);
        void RefreshGridRowTS(FileT file); 
        void TraceTS(string text); 
    }

    public partial class frmZiPreview : Form, IGuiUpdate, ImagesViewerData
    {
        private PropertyCache _properties;
        private ImagesViewer _images;
        private CPicture _image;

        private Panel _imagesPanel;
        private Panel _imagePanel;

        private int _noOfImages;

        private VideoCapture _videoCapture;

        //-----------------------------------------------------
        // ImagesViewerData
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
            return ((FileT)gridFiles.Rows[n].Tag).ImageFilename;
        }
        public string GetVideo(int n)
        {
            return ((FileT)gridFiles.Rows[n].Tag).VideoFilename;
        }

        //-----------------------------------------------------
        // IGuiUpdate
        //-----------------------------------------------------
        private delegate void VoidFileT(FileT file);
        private delegate void VoidString(string str);
        private delegate void VoidDataGridViewRow(DataGridViewRow row);
        private delegate void VoidVoid();

        public void TraceTS(string text)
        {
            if (InvokeRequired)
            {
                VoidString action = new VoidString(TraceTS);
                Invoke(action, new object[] { text });
            }
            else
            {
                // output trace
                listTrace.Items.Add(text);
            }
        }

        public void RefreshGridRowTS(FileT file)
        {
            if (InvokeRequired)
            {
                VoidFileT action = new VoidFileT(RefreshGridRowTS);
                Invoke(action, new object[] { file });
            }
            else
            {
                file.Row.Cells["colFilename"].Value = file.ShortFilename;
                file.Row.Cells["colSelected"].Value = file.SelectedS;
                file.Row.Cells["colType"].Value = file.TypeS;
                file.Row.Cells["colTimes"].Value = file.Times;
                file.Row.Cells["colDate"].Value = file.LastDate;
            }
        }

        public void AddFileToGridTS(FileT file)
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
                RefreshGridRowTS(file);
            }
        }


        //-------------------------------------------------------------------------------

        //-----------------------------------------------------
        // Form
        //-----------------------------------------------------
        public frmZiPreview()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // get password, quit on cancel
            string pwd = "";
            if (!Constants.TestMode)
            {
                pwd = frmPassword.GetPassword();
                if (pwd == "")
                {
                    Close();
                    return;
                }
            }

            Text = Constants.Title;
            _noOfImages = 2;

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

            _properties = new PropertyCache();

            // prep the video capture
            _videoCapture = new VideoCapture();
            _videoCapture.GuiUpdateIf = this;
            _videoCapture.Initialise(this);

            // set thread safe callback
            Files.GuiUpdateIf = this;
            ManageVHDs.GuiUpdateIf = this;

            SetupGrid();

            // load data and grid in background
            Thread tr = new Thread(() => PopulateGrid(pwd));
            tr.Start();
        }

        private void SetupGrid()
        {
            // grid initialise
            gridFiles.RowHeadersVisible = false;
            gridFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridFiles.MultiSelect = false;
            gridFiles.Rows.Clear();

            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Width = 170;
                col.HeaderText = "Filename";
                col.Name = "colFilename";
                gridFiles.Columns.Add(col);
            }
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Width = 15;
                col.HeaderText = "";
                col.Name = "colSelected";
                gridFiles.Columns.Add(col);
            }
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Width = 40;
                col.HeaderText = "Type";
                col.Name = "colType";
                gridFiles.Columns.Add(col);
            }
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Width = 80;
                col.HeaderText = "Date";
                col.Name = "colDate";
                gridFiles.Columns.Add(col);
            }
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Width = 80;
                col.HeaderText = "Time";
                col.Name = "colTimes";
                gridFiles.Columns.Add(col);
            }
        }

        private void PopulateGrid(string pwd)
        {
            GetFiles(pwd);
            LoadGrid();
            PopulateImageGridTS();
            TraceTS("Files: " + gridFiles.Rows.Count.ToString());
            Files.CreateImages();
        }

        private void GetFiles(string pwd)
        {
            Files.Clear();

            if (Constants.TestMode)
            {
                string[] fs = Directory.GetFiles(Constants.TestDir, "*.*", SearchOption.AllDirectories);
                Files.AddFiles(fs);

                _properties.AddDirectory(Constants.TestDir);
            }
            else
            {
                ManageVHDs.AttachAllVHDs(pwd);
                List<string> fs = ManageVHDs.GetAllFiles();
                Files.AddFiles(fs);

                List <string> drs = ManageVHDs.GetDrives();
                foreach (string dr in drs) _properties.AddDirectory(dr);
            }
        }
        private void LoadGrid()
        {
            List<FileT> files = Files.GetFiles();

            foreach (FileT file in files)
            {
                file.LastDate = _properties.GetProperty(file.VideoFilename, "lasttime");
                file.Times = _properties.GetProperty(file.VideoFilename, "times");

                AddFileToGridTS(file);
            }
        }

        private void GridFiles_SelectionChanged(object sender, EventArgs e)
        {
            SelectImagesViewer();
            var row = gridFiles.SelectedRows[0];
            FileT file = (FileT)row.Tag;
            _images.SetSelected(row.Index);
        }

        private void Dismount()
        {
            _properties.WriteProperties();
            ManageVHDs.UnattachAllVHDs();
        }

        private void GridFiles_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void GridFiles_KeyDown(object sender, KeyEventArgs e)
        {
            var rows = gridFiles.SelectedRows;
            if (rows.Count == 1)
            {
                FileT file = (FileT)rows[0].Tag;

                if (file != null)
                {
                    switch (e.KeyData)
                    {
                        case Keys.Add: ToggleSelect(file); e.SuppressKeyPress = true; break;
                        case Keys.Enter: PlayFile(file); e.SuppressKeyPress = true; break;
                        case Keys.Up: Up(file); e.SuppressKeyPress = true; break;
                        case Keys.Down: Down(file); e.SuppressKeyPress = true; break;
                        case Keys.PageDown: PageDown(file); e.SuppressKeyPress = true; break;
                        case Keys.PageUp: PageUp(file); e.SuppressKeyPress = true; break;
                        case Keys.Escape: SelectImagesViewer(); e.SuppressKeyPress = true; break;

                        case Keys.Right:
                        case Keys.Space:
                        case Keys.Z:
                        case Keys.V:
                        case Keys.X:
                        case Keys.C:
                            NextFile(file); e.SuppressKeyPress = true; break;

                        case Keys.Q:
                        case Keys.W:
                        case Keys.E:
                        case Keys.R:
                        case Keys.Left:
                            PreviousFile(file); e.SuppressKeyPress = true; break;

                        case Keys.F1: NextSelectedFile(file); e.SuppressKeyPress = true; break;
                        case Keys.F2: PreviousSelectedFile(file); e.SuppressKeyPress = true; break;

                        case Keys.F5: IncNoOfImages(file); e.SuppressKeyPress = true; break;
                        case Keys.F6: DecNoOfImages(file); e.SuppressKeyPress = true; break;
                        case Keys.F7: CaptureVideo(file); e.SuppressKeyPress = true; break;
                    }
                }
            }
        }
        private void IncNoOfImages(FileT file)
        {
            if (_noOfImages < 5)
            {
                _noOfImages++;
                _images.Initialise();
                _images.Draw(file.Row.Index);
                _images.SetSelected(file.Row.Index);
            }
        }
        private void DecNoOfImages(FileT file)
        {
            if (_noOfImages > 1)
            {
                _noOfImages--;
                _images.Initialise();
                _images.Draw(file.Row.Index);
                _images.SetSelected(file.Row.Index);
            }
        }

        private void NextSelectedFile(FileT file)
        {
            int r = file.Row.Index;
            int nr = r;

            while ((nr = ((nr + 1) % gridFiles.Rows.Count)) != r)
            {
                if (((FileT)gridFiles.Rows[nr].Tag).Selected)
                {
                    gridFiles.Rows[nr].Selected = true;
                    _images.SetSelected(nr);
                    break;
                }
            }
        }
        private void PreviousSelectedFile(FileT file)
        {
            int r = file.Row.Index;
            int nr = r;

            while ((nr = ((nr -1 + gridFiles.Rows.Count) % gridFiles.Rows.Count)) != r)
            {
                if (((FileT)gridFiles.Rows[nr].Tag).Selected)
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

        private void Up(FileT file)
        {
            int n = (file.Row.Index + gridFiles.Rows.Count - GetNoOfCols()) % gridFiles.Rows.Count;
            gridFiles.Rows[n].Selected = true;
        }

        private void Down(FileT file)
        {
            int n = (file.Row.Index + GetNoOfCols()) % gridFiles.Rows.Count;
            gridFiles.Rows[n].Selected = true;
        }

        private void PageUp(FileT file)
        {
            int n = (_images.Top + gridFiles.Rows.Count - (GetNoOfRows() * GetNoOfCols())) % gridFiles.Rows.Count;
            gridFiles.Rows[n].Selected = true;
        }
        private void PageDown(FileT file)
        {
            int n = (_images.Top + (GetNoOfRows() * GetNoOfCols())) % gridFiles.Rows.Count;
            gridFiles.Rows[n].Selected = true;
        }

        private void PreviousFile(FileT file)
        {
            int n = (file.Row.Index + gridFiles.Rows.Count - 1) % gridFiles.Rows.Count;
            gridFiles.Rows[n].Selected = true;
        }

        private void NextFile(FileT file)
        {
            int n = (file.Row.Index + gridFiles.Rows.Count + 1) % gridFiles.Rows.Count;
            gridFiles.Rows[n].Selected = true;
        }

        private void ToggleSelect(FileT file)
        {
            file.ToggleSelected();
            file.Row.Cells["colSelected"].Value = file.SelectedS;
        }

        private void CaptureVideo(FileT file)
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

                if (dr != DialogResult.OK) return;
            }

            _videoCapture.StartCapture(file);
        }

        private void GridFiles_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string col = gridFiles.Columns[e.ColumnIndex].Name;
            gridFiles.Sort(new FileTComparer(col));
            gridFiles.Rows[0].Selected = true;
        }

        public void PopulateImageGridTS()
        {
            if (InvokeRequired)
            {
                VoidVoid action = new VoidVoid(PopulateImageGridTS);
                Invoke(action);
            }
            else
            {
                _images.Draw(0);
            }
        }

        public void PlayFile(FileT file)
        {
            if (file.HasVideo)
            {
                _images.StopPreview();

                ProcessStartInfo psi = new ProcessStartInfo(file.VideoFilename);
                psi.UseShellExecute = true;
                psi.WindowStyle = ProcessWindowStyle.Maximized;
                Process.Start(psi);

                file.Times = _properties.IncCount(file.VideoFilename, "times");
                file.LastDate = _properties.DateStamp(file.VideoFilename, "lasttime");

                RefreshGridRowTS(file);
            }
            else if (file.HasLink)
            {
                Utilities.LaunchBrowser(file);
            }
            else if (file.HasImage)
            {
                _imagesPanel.Hide();
                _imagePanel.Show();

                // hide the images preview and show the image full size
                _image.LoadImage(file.ImageFilename);

                RefreshGridRowTS(file);
            }
        }

        private void Timer__Tick(object sender, EventArgs e)
        {
            CaptureClipboard.TimerEvent();
        }

        private void FileExitMenu_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmZiPreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CanClose();
        }

        private bool CanClose()
        { 
            _images.StopPreview();
            if (!Constants.TestMode)
            {
                DialogResult res = MessageBox.Show("Do you want to dismount the virtual drives ?",
                Constants.Title,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button3);

                switch (res)
                {
                    case DialogResult.Cancel: return false;
                    case DialogResult.Yes: Dismount(); break;
                    case DialogResult.No: _properties.WriteProperties(); break;
                }
            }
            else
            {
                _properties.WriteProperties();
            }
            _videoCapture.Uninitialise();
            return true;
        }

        private void ToolsCaptureSelectedMenu_Click(object sender, EventArgs e)
        {
            List<FileT> files = Files.GetFiles().FindAll(delegate (FileT file)
                { return file.Selected && file.HasLink; });

            if (files.Count == 0)
            {
                MessageBox.Show("No files with links have been selected", 
                    Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (FileT file in files)
            {
                CaptureVideo(file);
            }
        }

        private void ToolsCaptureWithoutMenu_Click(object sender, EventArgs e)
        {
            List<FileT> files = Files.GetFiles().FindAll(delegate (FileT file)
            { return !file.HasVideo && file.HasLink; });

            if (files.Count == 0)
            {
                MessageBox.Show("No files with links are missing videos",
                    Constants.Title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (FileT file in files)
            {
                CaptureVideo(file);
            }
        }
    }
}
