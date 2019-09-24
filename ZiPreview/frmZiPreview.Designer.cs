namespace ZiPreview
{
    partial class frmZiPreview
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitVertical = new System.Windows.Forms.SplitContainer();
            this.splitHorizGridTrace = new System.Windows.Forms.SplitContainer();
            this.gridFiles = new System.Windows.Forms.DataGridView();
            this.listTrace = new System.Windows.Forms.ListBox();
            this.timer_ = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.fileExitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsCaptureSelectedMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsCaptureWithoutMenu = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitVertical)).BeginInit();
            this.splitVertical.Panel1.SuspendLayout();
            this.splitVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitHorizGridTrace)).BeginInit();
            this.splitHorizGridTrace.Panel1.SuspendLayout();
            this.splitHorizGridTrace.Panel2.SuspendLayout();
            this.splitHorizGridTrace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitVertical
            // 
            this.splitVertical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitVertical.Location = new System.Drawing.Point(0, 24);
            this.splitVertical.Name = "splitVertical";
            // 
            // splitVertical.Panel1
            // 
            this.splitVertical.Panel1.Controls.Add(this.splitHorizGridTrace);
            this.splitVertical.Size = new System.Drawing.Size(1188, 635);
            this.splitVertical.SplitterDistance = 396;
            this.splitVertical.TabIndex = 0;
            // 
            // splitHorizGridTrace
            // 
            this.splitHorizGridTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitHorizGridTrace.Location = new System.Drawing.Point(0, 0);
            this.splitHorizGridTrace.Name = "splitHorizGridTrace";
            this.splitHorizGridTrace.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitHorizGridTrace.Panel1
            // 
            this.splitHorizGridTrace.Panel1.Controls.Add(this.gridFiles);
            // 
            // splitHorizGridTrace.Panel2
            // 
            this.splitHorizGridTrace.Panel2.Controls.Add(this.listTrace);
            this.splitHorizGridTrace.Size = new System.Drawing.Size(396, 635);
            this.splitHorizGridTrace.SplitterDistance = 478;
            this.splitHorizGridTrace.TabIndex = 0;
            // 
            // gridFiles
            // 
            this.gridFiles.AllowUserToAddRows = false;
            this.gridFiles.AllowUserToDeleteRows = false;
            this.gridFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFiles.Location = new System.Drawing.Point(0, 0);
            this.gridFiles.Name = "gridFiles";
            this.gridFiles.ReadOnly = true;
            this.gridFiles.Size = new System.Drawing.Size(396, 478);
            this.gridFiles.TabIndex = 0;
            this.gridFiles.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridFiles_ColumnHeaderMouseClick);
            this.gridFiles.SelectionChanged += new System.EventHandler(this.GridFiles_SelectionChanged);
            this.gridFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridFiles_KeyDown);
            this.gridFiles.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GridFiles_KeyUp);
            // 
            // listTrace
            // 
            this.listTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTrace.FormattingEnabled = true;
            this.listTrace.Location = new System.Drawing.Point(0, 0);
            this.listTrace.Name = "listTrace";
            this.listTrace.Size = new System.Drawing.Size(396, 153);
            this.listTrace.TabIndex = 0;
            // 
            // timer_
            // 
            this.timer_.Enabled = true;
            this.timer_.Interval = 1000;
            this.timer_.Tick += new System.EventHandler(this.Timer__Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.toolsMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1188, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileExitMenu});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "&File";
            // 
            // fileExitMenu
            // 
            this.fileExitMenu.Name = "fileExitMenu";
            this.fileExitMenu.Size = new System.Drawing.Size(180, 22);
            this.fileExitMenu.Text = "E&xit";
            this.fileExitMenu.Click += new System.EventHandler(this.FileExitMenu_Click);
            // 
            // toolsMenu
            // 
            this.toolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsCaptureSelectedMenu,
            this.toolsCaptureWithoutMenu});
            this.toolsMenu.Name = "toolsMenu";
            this.toolsMenu.Size = new System.Drawing.Size(47, 20);
            this.toolsMenu.Text = "Tools";
            // 
            // toolsCaptureSelectedMenu
            // 
            this.toolsCaptureSelectedMenu.Name = "toolsCaptureSelectedMenu";
            this.toolsCaptureSelectedMenu.Size = new System.Drawing.Size(305, 22);
            this.toolsCaptureSelectedMenu.Text = "Capture video for selected items";
            this.toolsCaptureSelectedMenu.Click += new System.EventHandler(this.ToolsCaptureSelectedMenu_Click);
            // 
            // toolsCaptureWithoutMenu
            // 
            this.toolsCaptureWithoutMenu.Name = "toolsCaptureWithoutMenu";
            this.toolsCaptureWithoutMenu.Size = new System.Drawing.Size(305, 22);
            this.toolsCaptureWithoutMenu.Text = "Capture video for those items with no video";
            this.toolsCaptureWithoutMenu.Click += new System.EventHandler(this.ToolsCaptureWithoutMenu_Click);
            // 
            // frmZiPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1188, 659);
            this.Controls.Add(this.splitVertical);
            this.Controls.Add(this.menuStrip1);
            this.Name = "frmZiPreview";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmZiPreview_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitVertical.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitVertical)).EndInit();
            this.splitVertical.ResumeLayout(false);
            this.splitHorizGridTrace.Panel1.ResumeLayout(false);
            this.splitHorizGridTrace.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitHorizGridTrace)).EndInit();
            this.splitHorizGridTrace.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitVertical;
        private System.Windows.Forms.SplitContainer splitHorizGridTrace;
        private System.Windows.Forms.DataGridView gridFiles;
        private System.Windows.Forms.ListBox listTrace;
        private System.Windows.Forms.Timer timer_;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem fileExitMenu;
        private System.Windows.Forms.ToolStripMenuItem toolsMenu;
        private System.Windows.Forms.ToolStripMenuItem toolsCaptureSelectedMenu;
        private System.Windows.Forms.ToolStripMenuItem toolsCaptureWithoutMenu;
    }
}

