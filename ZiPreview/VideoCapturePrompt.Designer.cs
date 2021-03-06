﻿namespace ZiPreview
{
    partial class VideoCapturePrompt
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
            this.lblURL = new System.Windows.Forms.Label();
            this.textInstructions = new System.Windows.Forms.TextBox();
            this.chkMuteDuring = new System.Windows.Forms.CheckBox();
            this.chkUnmuteAfter = new System.Windows.Forms.CheckBox();
            this.chkDoNotShow = new System.Windows.Forms.CheckBox();
            this.chkLaunchBrowser = new System.Windows.Forms.CheckBox();
            this.chkCloseBrowser = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Location = new System.Drawing.Point(17, 440);
            this.butOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(112, 35);
            this.butOK.TabIndex = 0;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.ButOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(537, 440);
            this.butCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(112, 35);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(19, 20);
            this.lblURL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(42, 20);
            this.lblURL.TabIndex = 2;
            this.lblURL.Text = "URL";
            // 
            // textInstructions
            // 
            this.textInstructions.BackColor = System.Drawing.SystemColors.Control;
            this.textInstructions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textInstructions.Location = new System.Drawing.Point(17, 72);
            this.textInstructions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textInstructions.Multiline = true;
            this.textInstructions.Name = "textInstructions";
            this.textInstructions.ReadOnly = true;
            this.textInstructions.Size = new System.Drawing.Size(642, 194);
            this.textInstructions.TabIndex = 3;
            // 
            // chkMuteDuring
            // 
            this.chkMuteDuring.AutoSize = true;
            this.chkMuteDuring.Checked = true;
            this.chkMuteDuring.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMuteDuring.Location = new System.Drawing.Point(17, 294);
            this.chkMuteDuring.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkMuteDuring.Name = "chkMuteDuring";
            this.chkMuteDuring.Size = new System.Drawing.Size(232, 24);
            this.chkMuteDuring.TabIndex = 4;
            this.chkMuteDuring.Text = "Mute audio during recording";
            this.chkMuteDuring.UseVisualStyleBackColor = true;
            // 
            // chkUnmuteAfter
            // 
            this.chkUnmuteAfter.AutoSize = true;
            this.chkUnmuteAfter.Checked = true;
            this.chkUnmuteAfter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUnmuteAfter.Location = new System.Drawing.Point(17, 329);
            this.chkUnmuteAfter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkUnmuteAfter.Name = "chkUnmuteAfter";
            this.chkUnmuteAfter.Size = new System.Drawing.Size(326, 24);
            this.chkUnmuteAfter.TabIndex = 5;
            this.chkUnmuteAfter.Text = "Unmute audio after recording is complete";
            this.chkUnmuteAfter.UseVisualStyleBackColor = true;
            // 
            // chkDoNotShow
            // 
            this.chkDoNotShow.AutoSize = true;
            this.chkDoNotShow.Location = new System.Drawing.Point(454, 329);
            this.chkDoNotShow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkDoNotShow.Name = "chkDoNotShow";
            this.chkDoNotShow.Size = new System.Drawing.Size(196, 24);
            this.chkDoNotShow.TabIndex = 6;
            this.chkDoNotShow.Text = "Do not show this again";
            this.chkDoNotShow.UseVisualStyleBackColor = true;
            this.chkDoNotShow.CheckedChanged += new System.EventHandler(this.ChkDoNotShow_CheckedChanged);
            // 
            // chkLaunchBrowser
            // 
            this.chkLaunchBrowser.AutoSize = true;
            this.chkLaunchBrowser.Checked = true;
            this.chkLaunchBrowser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLaunchBrowser.Location = new System.Drawing.Point(17, 364);
            this.chkLaunchBrowser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkLaunchBrowser.Name = "chkLaunchBrowser";
            this.chkLaunchBrowser.Size = new System.Drawing.Size(150, 24);
            this.chkLaunchBrowser.TabIndex = 7;
            this.chkLaunchBrowser.Text = "Launch Browser";
            this.chkLaunchBrowser.UseVisualStyleBackColor = true;
            this.chkLaunchBrowser.CheckedChanged += new System.EventHandler(this.ChkLaunchBrowser_CheckedChanged);
            // 
            // chkCloseBrowser
            // 
            this.chkCloseBrowser.AutoSize = true;
            this.chkCloseBrowser.Checked = true;
            this.chkCloseBrowser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCloseBrowser.Location = new System.Drawing.Point(17, 398);
            this.chkCloseBrowser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkCloseBrowser.Name = "chkCloseBrowser";
            this.chkCloseBrowser.Size = new System.Drawing.Size(135, 24);
            this.chkCloseBrowser.TabIndex = 8;
            this.chkCloseBrowser.Text = "Close browser";
            this.chkCloseBrowser.UseVisualStyleBackColor = true;
            // 
            // VideoCapturePrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 491);
            this.Controls.Add(this.chkCloseBrowser);
            this.Controls.Add(this.chkLaunchBrowser);
            this.Controls.Add(this.chkDoNotShow);
            this.Controls.Add(this.chkUnmuteAfter);
            this.Controls.Add(this.chkMuteDuring);
            this.Controls.Add(this.textInstructions);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VideoCapturePrompt";
            this.Text = "Start Video Capture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoCapturePrompt_FormClosing);
            this.Load += new System.EventHandler(this.VideoCapturePrompt_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox textInstructions;
        private System.Windows.Forms.CheckBox chkMuteDuring;
        private System.Windows.Forms.CheckBox chkUnmuteAfter;
        private System.Windows.Forms.CheckBox chkDoNotShow;
        private System.Windows.Forms.CheckBox chkLaunchBrowser;
        private System.Windows.Forms.CheckBox chkCloseBrowser;
    }
}