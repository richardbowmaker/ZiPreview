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
            ((System.ComponentModel.ISupportInitialize)(this.splitVertical)).BeginInit();
            this.splitVertical.Panel1.SuspendLayout();
            this.splitVertical.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitHorizGridTrace)).BeginInit();
            this.splitHorizGridTrace.Panel1.SuspendLayout();
            this.splitHorizGridTrace.Panel2.SuspendLayout();
            this.splitHorizGridTrace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // splitVertical
            // 
            this.splitVertical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitVertical.Location = new System.Drawing.Point(0, 0);
            this.splitVertical.Name = "splitVertical";
            // 
            // splitVertical.Panel1
            // 
            this.splitVertical.Panel1.Controls.Add(this.splitHorizGridTrace);
            this.splitVertical.Size = new System.Drawing.Size(1188, 659);
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
            this.splitHorizGridTrace.Size = new System.Drawing.Size(396, 659);
            this.splitHorizGridTrace.SplitterDistance = 497;
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
            this.gridFiles.Size = new System.Drawing.Size(396, 497);
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
            this.listTrace.Size = new System.Drawing.Size(396, 158);
            this.listTrace.TabIndex = 0;
            // 
            // timer_
            // 
            this.timer_.Enabled = true;
            this.timer_.Interval = 1000;
            this.timer_.Tick += new System.EventHandler(this.Timer__Tick);
            // 
            // frmZiPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1188, 659);
            this.Controls.Add(this.splitVertical);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitVertical;
        private System.Windows.Forms.SplitContainer splitHorizGridTrace;
        private System.Windows.Forms.DataGridView gridFiles;
        private System.Windows.Forms.ListBox listTrace;
        private System.Windows.Forms.Timer timer_;
    }
}

