namespace ZiPreview
{
    partial class CheckImageLink
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
            this.lblUrl = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butOK.Location = new System.Drawing.Point(16, 308);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 0;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.ButOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(322, 308);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // lblUrl
            // 
            this.lblUrl.AutoSize = true;
            this.lblUrl.Location = new System.Drawing.Point(13, 13);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(0, 13);
            this.lblUrl.TabIndex = 2;
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Location = new System.Drawing.Point(60, 42);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(287, 238);
            this.panel.TabIndex = 3;
            // 
            // CheckImageLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 343);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CheckImageLink";
            this.Text = "CheckImageLink";
            this.Load += new System.EventHandler(this.CheckImageLink_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.Panel panel;
    }
}