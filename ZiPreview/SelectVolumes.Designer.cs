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
            this.butSelectAll = new System.Windows.Forms.Button();
            this.butDeselectAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // butOK
            // 
            this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.butOK.Location = new System.Drawing.Point(20, 509);
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
            this.butCancel.Location = new System.Drawing.Point(635, 509);
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
            this.lblVeracrypt.Location = new System.Drawing.Point(357, 16);
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
            this.chksBitLocker.Size = new System.Drawing.Size(316, 454);
            this.chksBitLocker.TabIndex = 4;
            this.chksBitLocker.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChksBitLocker_ItemCheck);
            // 
            // chksVeracrypt
            // 
            this.chksVeracrypt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chksVeracrypt.CheckOnClick = true;
            this.chksVeracrypt.FormattingEnabled = true;
            this.chksVeracrypt.Location = new System.Drawing.Point(360, 32);
            this.chksVeracrypt.Name = "chksVeracrypt";
            this.chksVeracrypt.Size = new System.Drawing.Size(350, 454);
            this.chksVeracrypt.TabIndex = 5;
            this.chksVeracrypt.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ChksVeracrypt_ItemCheck);
            // 
            // butSelectAll
            // 
            this.butSelectAll.Location = new System.Drawing.Point(231, 509);
            this.butSelectAll.Name = "butSelectAll";
            this.butSelectAll.Size = new System.Drawing.Size(66, 22);
            this.butSelectAll.TabIndex = 6;
            this.butSelectAll.Text = "Select All";
            this.butSelectAll.UseVisualStyleBackColor = true;
            this.butSelectAll.Click += new System.EventHandler(this.ButSelectAll_Click);
            // 
            // butDeselectAll
            // 
            this.butDeselectAll.Location = new System.Drawing.Point(410, 509);
            this.butDeselectAll.Name = "butDeselectAll";
            this.butDeselectAll.Size = new System.Drawing.Size(82, 23);
            this.butDeselectAll.TabIndex = 7;
            this.butDeselectAll.Text = "Deselect All";
            this.butDeselectAll.UseVisualStyleBackColor = true;
            this.butDeselectAll.Click += new System.EventHandler(this.ButDeselectAll_Click);
            // 
            // SelectVolumes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 544);
            this.Controls.Add(this.butDeselectAll);
            this.Controls.Add(this.butSelectAll);
            this.Controls.Add(this.chksVeracrypt);
            this.Controls.Add(this.chksBitLocker);
            this.Controls.Add(this.lblVeracrypt);
            this.Controls.Add(this.lblBitLocker);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectVolumes";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
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
        private System.Windows.Forms.Button butSelectAll;
        private System.Windows.Forms.Button butDeselectAll;
    }
}