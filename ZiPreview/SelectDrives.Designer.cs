namespace ZiPreview
{
    partial class SelectDrives
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
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.butFolder = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.chkFiles = new System.Windows.Forms.CheckedListBox();
            this.butOk = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.butFind = new System.Windows.Forms.Button();
            this.butSelectAll = new System.Windows.Forms.Button();
            this.butDeselectAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(31, 27);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.ReadOnly = true;
            this.txtFolder.Size = new System.Drawing.Size(686, 22);
            this.txtFolder.TabIndex = 0;
            // 
            // butFolder
            // 
            this.butFolder.Location = new System.Drawing.Point(749, 28);
            this.butFolder.Name = "butFolder";
            this.butFolder.Size = new System.Drawing.Size(31, 23);
            this.butFolder.TabIndex = 1;
            this.butFolder.Text = "...";
            this.butFolder.UseVisualStyleBackColor = true;
            this.butFolder.Click += new System.EventHandler(this.butFolder_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(31, 65);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(222, 22);
            this.txtFilter.TabIndex = 2;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // chkFiles
            // 
            this.chkFiles.CheckOnClick = true;
            this.chkFiles.FormattingEnabled = true;
            this.chkFiles.Location = new System.Drawing.Point(31, 108);
            this.chkFiles.Name = "chkFiles";
            this.chkFiles.Size = new System.Drawing.Size(573, 378);
            this.chkFiles.TabIndex = 3;
            // 
            // butOk
            // 
            this.butOk.Location = new System.Drawing.Point(637, 458);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(89, 28);
            this.butOk.TabIndex = 4;
            this.butOk.Text = "OK";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(888, 463);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(89, 28);
            this.butCancel.TabIndex = 5;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(637, 390);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(339, 30);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // butFind
            // 
            this.butFind.Location = new System.Drawing.Point(641, 119);
            this.butFind.Name = "butFind";
            this.butFind.Size = new System.Drawing.Size(75, 39);
            this.butFind.TabIndex = 8;
            this.butFind.Text = "Find";
            this.butFind.UseVisualStyleBackColor = true;
            this.butFind.Click += new System.EventHandler(this.butFind_Click);
            // 
            // butSelectAll
            // 
            this.butSelectAll.Location = new System.Drawing.Point(641, 186);
            this.butSelectAll.Name = "butSelectAll";
            this.butSelectAll.Size = new System.Drawing.Size(114, 37);
            this.butSelectAll.TabIndex = 9;
            this.butSelectAll.Text = "Select All";
            this.butSelectAll.UseVisualStyleBackColor = true;
            this.butSelectAll.Click += new System.EventHandler(this.butSelectAll_Click);
            // 
            // butDeselectAll
            // 
            this.butDeselectAll.Location = new System.Drawing.Point(641, 250);
            this.butDeselectAll.Name = "butDeselectAll";
            this.butDeselectAll.Size = new System.Drawing.Size(111, 37);
            this.butDeselectAll.TabIndex = 10;
            this.butDeselectAll.Text = "Deselect All";
            this.butDeselectAll.UseVisualStyleBackColor = true;
            this.butDeselectAll.Click += new System.EventHandler(this.butDeselectAll_Click);
            // 
            // SelectDrives
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 503);
            this.Controls.Add(this.butDeselectAll);
            this.Controls.Add(this.butSelectAll);
            this.Controls.Add(this.butFind);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.chkFiles);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.butFolder);
            this.Controls.Add(this.txtFolder);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectDrives";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SelectDrives";
            this.Load += new System.EventHandler(this.SelectDrives_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Button butFolder;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.CheckedListBox chkFiles;
        private System.Windows.Forms.Button butOk;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button butFind;
        private System.Windows.Forms.Button butSelectAll;
        private System.Windows.Forms.Button butDeselectAll;
    }
}