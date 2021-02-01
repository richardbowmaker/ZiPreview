namespace ZiPreview
{
    partial class FileImport
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
            this.butAdd = new System.Windows.Forms.Button();
            this.butImport = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.listFiles = new System.Windows.Forms.ListBox();
            this.chkRemoveAudio = new System.Windows.Forms.CheckBox();
            this.chkGenerateImage = new System.Windows.Forms.CheckBox();
            this.chkDelete = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // butAdd
            // 
            this.butAdd.Location = new System.Drawing.Point(23, 29);
            this.butAdd.Name = "butAdd";
            this.butAdd.Size = new System.Drawing.Size(97, 35);
            this.butAdd.TabIndex = 0;
            this.butAdd.Text = "Add ...";
            this.butAdd.UseVisualStyleBackColor = true;
            this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
            // 
            // butImport
            // 
            this.butImport.Location = new System.Drawing.Point(12, 648);
            this.butImport.Name = "butImport";
            this.butImport.Size = new System.Drawing.Size(133, 41);
            this.butImport.TabIndex = 1;
            this.butImport.Text = "Import";
            this.butImport.UseVisualStyleBackColor = true;
            this.butImport.Click += new System.EventHandler(this.butImport_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(549, 649);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(125, 38);
            this.butCancel.TabIndex = 2;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // listFiles
            // 
            this.listFiles.FormattingEnabled = true;
            this.listFiles.ItemHeight = 20;
            this.listFiles.Location = new System.Drawing.Point(23, 84);
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size(651, 384);
            this.listFiles.TabIndex = 3;
            // 
            // chkRemoveAudio
            // 
            this.chkRemoveAudio.AutoSize = true;
            this.chkRemoveAudio.Checked = true;
            this.chkRemoveAudio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRemoveAudio.Location = new System.Drawing.Point(23, 501);
            this.chkRemoveAudio.Name = "chkRemoveAudio";
            this.chkRemoveAudio.Size = new System.Drawing.Size(139, 24);
            this.chkRemoveAudio.TabIndex = 4;
            this.chkRemoveAudio.Text = "Remove Audio";
            this.chkRemoveAudio.UseVisualStyleBackColor = true;
            // 
            // chkGenerateImage
            // 
            this.chkGenerateImage.AutoSize = true;
            this.chkGenerateImage.Checked = true;
            this.chkGenerateImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerateImage.Location = new System.Drawing.Point(23, 585);
            this.chkGenerateImage.Name = "chkGenerateImage";
            this.chkGenerateImage.Size = new System.Drawing.Size(150, 24);
            this.chkGenerateImage.TabIndex = 5;
            this.chkGenerateImage.Text = "Generate image";
            this.chkGenerateImage.UseVisualStyleBackColor = true;
            // 
            // chkDelete
            // 
            this.chkDelete.AutoSize = true;
            this.chkDelete.Checked = true;
            this.chkDelete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDelete.Location = new System.Drawing.Point(23, 542);
            this.chkDelete.Name = "chkDelete";
            this.chkDelete.Size = new System.Drawing.Size(134, 24);
            this.chkDelete.TabIndex = 6;
            this.chkDelete.Text = "Delete source";
            this.chkDelete.UseVisualStyleBackColor = true;
            // 
            // FileImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 695);
            this.Controls.Add(this.chkDelete);
            this.Controls.Add(this.chkGenerateImage);
            this.Controls.Add(this.chkRemoveAudio);
            this.Controls.Add(this.listFiles);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butImport);
            this.Controls.Add(this.butAdd);
            this.Name = "FileImport";
            this.Text = "FileImport";
            this.Load += new System.EventHandler(this.FileImport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butAdd;
        private System.Windows.Forms.Button butImport;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.ListBox listFiles;
        private System.Windows.Forms.CheckBox chkRemoveAudio;
        private System.Windows.Forms.CheckBox chkGenerateImage;
        private System.Windows.Forms.CheckBox chkDelete;
    }
}