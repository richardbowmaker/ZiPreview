namespace ZiPreview
{
    partial class VerifyDelete
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
            this.butDelete = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.chkImage = new System.Windows.Forms.CheckBox();
            this.chkLink = new System.Windows.Forms.CheckBox();
            this.chkVideo = new System.Windows.Forms.CheckBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblLink = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // butDelete
            // 
            this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butDelete.Location = new System.Drawing.Point(13, 374);
            this.butDelete.Name = "butDelete";
            this.butDelete.Size = new System.Drawing.Size(75, 23);
            this.butDelete.TabIndex = 0;
            this.butDelete.Text = "Delete";
            this.butDelete.UseVisualStyleBackColor = true;
            this.butDelete.Click += new System.EventHandler(this.ButDelete_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(511, 374);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Location = new System.Drawing.Point(13, 65);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(449, 292);
            this.panel.TabIndex = 2;
            // 
            // chkImage
            // 
            this.chkImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkImage.AutoSize = true;
            this.chkImage.Location = new System.Drawing.Point(479, 222);
            this.chkImage.Name = "chkImage";
            this.chkImage.Size = new System.Drawing.Size(88, 17);
            this.chkImage.TabIndex = 3;
            this.chkImage.Text = "Delete image";
            this.chkImage.UseVisualStyleBackColor = true;
            this.chkImage.CheckedChanged += new System.EventHandler(this.ChkImage_CheckedChanged);
            // 
            // chkLink
            // 
            this.chkLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLink.AutoSize = true;
            this.chkLink.Location = new System.Drawing.Point(479, 284);
            this.chkLink.Name = "chkLink";
            this.chkLink.Size = new System.Drawing.Size(76, 17);
            this.chkLink.TabIndex = 4;
            this.chkLink.Text = "Delete link";
            this.chkLink.UseVisualStyleBackColor = true;
            this.chkLink.CheckedChanged += new System.EventHandler(this.ChkLink_CheckedChanged);
            // 
            // chkVideo
            // 
            this.chkVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkVideo.AutoSize = true;
            this.chkVideo.Location = new System.Drawing.Point(479, 253);
            this.chkVideo.Name = "chkVideo";
            this.chkVideo.Size = new System.Drawing.Size(86, 17);
            this.chkVideo.TabIndex = 5;
            this.chkVideo.Text = "Delete video";
            this.chkVideo.UseVisualStyleBackColor = true;
            this.chkVideo.CheckedChanged += new System.EventHandler(this.ChkVideo_CheckedChanged);
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(13, 13);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(23, 13);
            this.lblFile.TabIndex = 6;
            this.lblFile.Text = "File";
            // 
            // lblLink
            // 
            this.lblLink.AutoSize = true;
            this.lblLink.Location = new System.Drawing.Point(13, 35);
            this.lblLink.Name = "lblLink";
            this.lblLink.Size = new System.Drawing.Size(27, 13);
            this.lblLink.TabIndex = 7;
            this.lblLink.Text = "Link";
            // 
            // VerifyDelete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 409);
            this.Controls.Add(this.lblLink);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.chkVideo);
            this.Controls.Add(this.chkLink);
            this.Controls.Add(this.chkImage);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butDelete);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VerifyDelete";
            this.Text = "VerifyDelete";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VerifyDelete_FormClosing);
            this.Load += new System.EventHandler(this.VerifyDelete_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butDelete;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.CheckBox chkImage;
        private System.Windows.Forms.CheckBox chkLink;
        private System.Windows.Forms.CheckBox chkVideo;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblLink;
    }
}