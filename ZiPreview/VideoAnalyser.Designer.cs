namespace ZiPreview
{
    partial class VideoAnalyser
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
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.lblVolumes = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVolAverage = new System.Windows.Forms.TextBox();
            this.txtVolPeak = new System.Windows.Forms.TextBox();
            this.chkAdjustVolume = new System.Windows.Forms.CheckBox();
            this.volUpDown = new System.Windows.Forms.NumericUpDown();
            this.chkCompress = new System.Windows.Forms.CheckBox();
            this.chkNewImage = new System.Windows.Forms.CheckBox();
            this.chkRemoveAudio = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.volUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(12, 231);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(91, 32);
            this.butOK.TabIndex = 0;
            this.butOK.Text = "Process";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.butOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(215, 231);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(85, 32);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // lblVolumes
            // 
            this.lblVolumes.AutoSize = true;
            this.lblVolumes.Location = new System.Drawing.Point(12, 20);
            this.lblVolumes.Name = "lblVolumes";
            this.lblVolumes.Size = new System.Drawing.Size(83, 17);
            this.lblVolumes.TabIndex = 2;
            this.lblVolumes.Text = "Volumes dB";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Average";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(172, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Peak";
            // 
            // txtVolAverage
            // 
            this.txtVolAverage.Location = new System.Drawing.Point(79, 58);
            this.txtVolAverage.Name = "txtVolAverage";
            this.txtVolAverage.ReadOnly = true;
            this.txtVolAverage.Size = new System.Drawing.Size(71, 22);
            this.txtVolAverage.TabIndex = 5;
            // 
            // txtVolPeak
            // 
            this.txtVolPeak.Location = new System.Drawing.Point(218, 58);
            this.txtVolPeak.Name = "txtVolPeak";
            this.txtVolPeak.ReadOnly = true;
            this.txtVolPeak.Size = new System.Drawing.Size(82, 22);
            this.txtVolPeak.TabIndex = 6;
            // 
            // chkAdjustVolume
            // 
            this.chkAdjustVolume.AutoSize = true;
            this.chkAdjustVolume.Location = new System.Drawing.Point(16, 101);
            this.chkAdjustVolume.Name = "chkAdjustVolume";
            this.chkAdjustVolume.Size = new System.Drawing.Size(120, 21);
            this.chkAdjustVolume.TabIndex = 7;
            this.chkAdjustVolume.Text = "Adjust Volume";
            this.chkAdjustVolume.UseVisualStyleBackColor = true;
            this.chkAdjustVolume.CheckedChanged += new System.EventHandler(this.chkAdjustVolume_CheckedChanged);
            // 
            // volUpDown
            // 
            this.volUpDown.DecimalPlaces = 1;
            this.volUpDown.Location = new System.Drawing.Point(157, 101);
            this.volUpDown.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.volUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147418112});
            this.volUpDown.Name = "volUpDown";
            this.volUpDown.Size = new System.Drawing.Size(55, 22);
            this.volUpDown.TabIndex = 8;
            // 
            // chkCompress
            // 
            this.chkCompress.AutoSize = true;
            this.chkCompress.Location = new System.Drawing.Point(16, 159);
            this.chkCompress.Name = "chkCompress";
            this.chkCompress.Size = new System.Drawing.Size(93, 21);
            this.chkCompress.TabIndex = 9;
            this.chkCompress.Text = "Compress";
            this.chkCompress.UseVisualStyleBackColor = true;
            this.chkCompress.CheckedChanged += new System.EventHandler(this.chkCompress_CheckedChanged);
            // 
            // chkNewImage
            // 
            this.chkNewImage.AutoSize = true;
            this.chkNewImage.Location = new System.Drawing.Point(16, 186);
            this.chkNewImage.Name = "chkNewImage";
            this.chkNewImage.Size = new System.Drawing.Size(99, 21);
            this.chkNewImage.TabIndex = 10;
            this.chkNewImage.Text = "New Image";
            this.chkNewImage.UseVisualStyleBackColor = true;
            this.chkNewImage.CheckedChanged += new System.EventHandler(this.chkNewImage_CheckedChanged);
            // 
            // chkRemoveAudio
            // 
            this.chkRemoveAudio.AutoSize = true;
            this.chkRemoveAudio.Location = new System.Drawing.Point(15, 128);
            this.chkRemoveAudio.Name = "chkRemoveAudio";
            this.chkRemoveAudio.Size = new System.Drawing.Size(122, 21);
            this.chkRemoveAudio.TabIndex = 11;
            this.chkRemoveAudio.Text = "Remove Audio";
            this.chkRemoveAudio.UseVisualStyleBackColor = true;
            this.chkRemoveAudio.CheckedChanged += new System.EventHandler(this.chkRemoveAudio_CheckedChanged);
            // 
            // VideoAnalyser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 277);
            this.Controls.Add(this.chkRemoveAudio);
            this.Controls.Add(this.chkNewImage);
            this.Controls.Add(this.chkCompress);
            this.Controls.Add(this.volUpDown);
            this.Controls.Add(this.chkAdjustVolume);
            this.Controls.Add(this.txtVolPeak);
            this.Controls.Add(this.txtVolAverage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblVolumes);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VideoAnalyser";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "VideoAnalyser";
            this.Load += new System.EventHandler(this.VideoAnalyser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.volUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label lblVolumes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtVolAverage;
        private System.Windows.Forms.TextBox txtVolPeak;
        private System.Windows.Forms.CheckBox chkAdjustVolume;
        private System.Windows.Forms.NumericUpDown volUpDown;
        private System.Windows.Forms.CheckBox chkCompress;
        private System.Windows.Forms.CheckBox chkNewImage;
        private System.Windows.Forms.CheckBox chkRemoveAudio;
    }
}