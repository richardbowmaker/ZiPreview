namespace ZiPreview
{
    partial class SelectVolumes
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
            this.lblBitLocker = new System.Windows.Forms.Label();
            this.lblVeracrypt = new System.Windows.Forms.Label();
            this.chksBitLocker = new System.Windows.Forms.CheckedListBox();
            this.chksVeracrypt = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butOK.Location = new System.Drawing.Point(20, 377);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(75, 23);
            this.butOK.TabIndex = 0;
            this.butOK.Text = "Mount";
            this.butOK.UseVisualStyleBackColor = true;
            this.butOK.Click += new System.EventHandler(this.ButOK_Click);
            // 
            // butCancel
            // 
            this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butCancel.Location = new System.Drawing.Point(414, 377);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // lblBitLocker
            // 
            this.lblBitLocker.AutoSize = true;
            this.lblBitLocker.Location = new System.Drawing.Point(17, 16);
            this.lblBitLocker.Name = "lblBitLocker";
            this.lblBitLocker.Size = new System.Drawing.Size(58, 13);
            this.lblBitLocker.TabIndex = 2;
            this.lblBitLocker.Text = "Bit Locker:";
            // 
            // lblVeracrypt
            // 
            this.lblVeracrypt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVeracrypt.AutoSize = true;
            this.lblVeracrypt.Location = new System.Drawing.Point(264, 16);
            this.lblVeracrypt.Name = "lblVeracrypt";
            this.lblVeracrypt.Size = new System.Drawing.Size(55, 13);
            this.lblVeracrypt.TabIndex = 3;
            this.lblVeracrypt.Text = "Veracrypt:";
            // 
            // chksBitLocker
            // 
            this.chksBitLocker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chksBitLocker.CheckOnClick = true;
            this.chksBitLocker.FormattingEnabled = true;
            this.chksBitLocker.Location = new System.Drawing.Point(20, 32);
            this.chksBitLocker.Name = "chksBitLocker";
            this.chksBitLocker.Size = new System.Drawing.Size(222, 334);
            this.chksBitLocker.TabIndex = 4;
            // 
            // chksVeracrypt
            // 
            this.chksVeracrypt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chksVeracrypt.CheckOnClick = true;
            this.chksVeracrypt.FormattingEnabled = true;
            this.chksVeracrypt.Location = new System.Drawing.Point(267, 32);
            this.chksVeracrypt.Name = "chksVeracrypt";
            this.chksVeracrypt.Size = new System.Drawing.Size(222, 334);
            this.chksVeracrypt.TabIndex = 5;
            this.chksVeracrypt.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChksVeracrypt_ItemCheck);
            // 
            // SelectVolumes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 412);
            this.Controls.Add(this.chksVeracrypt);
            this.Controls.Add(this.chksBitLocker);
            this.Controls.Add(this.lblVeracrypt);
            this.Controls.Add(this.lblBitLocker);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Name = "SelectVolumes";
            this.Text = "SelectVolumes";
            this.Load += new System.EventHandler(this.SelectVolumes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label lblBitLocker;
        private System.Windows.Forms.Label lblVeracrypt;
        private System.Windows.Forms.CheckedListBox chksBitLocker;
        private System.Windows.Forms.CheckedListBox chksVeracrypt;
    }
}