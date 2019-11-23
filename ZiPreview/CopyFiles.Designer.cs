namespace ZiPreview
{
    partial class CopyFiles
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
            this.listBoxSourceFolders = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.butAddSourceFolder = new System.Windows.Forms.Button();
            this.checkedListBoxDestFolders = new System.Windows.Forms.CheckedListBox();
            this.butCancel = new System.Windows.Forms.Button();
            this.butCopy = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxSourceFolders
            // 
            this.listBoxSourceFolders.FormattingEnabled = true;
            this.listBoxSourceFolders.Location = new System.Drawing.Point(10, 41);
            this.listBoxSourceFolders.Name = "listBoxSourceFolders";
            this.listBoxSourceFolders.Size = new System.Drawing.Size(313, 277);
            this.listBoxSourceFolders.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Source Folders:";
            // 
            // butAddSourceFolder
            // 
            this.butAddSourceFolder.Location = new System.Drawing.Point(107, 8);
            this.butAddSourceFolder.Name = "butAddSourceFolder";
            this.butAddSourceFolder.Size = new System.Drawing.Size(81, 23);
            this.butAddSourceFolder.TabIndex = 2;
            this.butAddSourceFolder.Text = "Add Folder";
            this.butAddSourceFolder.UseVisualStyleBackColor = true;
            this.butAddSourceFolder.Click += new System.EventHandler(this.ButAddSourceFolder_Click);
            // 
            // checkedListBoxDestFolders
            // 
            this.checkedListBoxDestFolders.CheckOnClick = true;
            this.checkedListBoxDestFolders.FormattingEnabled = true;
            this.checkedListBoxDestFolders.Location = new System.Drawing.Point(344, 41);
            this.checkedListBoxDestFolders.Name = "checkedListBoxDestFolders";
            this.checkedListBoxDestFolders.Size = new System.Drawing.Size(363, 274);
            this.checkedListBoxDestFolders.TabIndex = 3;
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(632, 324);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 5;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // butCopy
            // 
            this.butCopy.Location = new System.Drawing.Point(12, 324);
            this.butCopy.Name = "butCopy";
            this.butCopy.Size = new System.Drawing.Size(88, 23);
            this.butCopy.TabIndex = 6;
            this.butCopy.Text = "Start Copy";
            this.butCopy.UseVisualStyleBackColor = true;
            this.butCopy.Click += new System.EventHandler(this.ButCopy_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(344, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Destination folders:";
            // 
            // CopyFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 356);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.butCopy);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.checkedListBoxDestFolders);
            this.Controls.Add(this.butAddSourceFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxSourceFolders);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopyFiles";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "CopyFiles";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CopyFiles_FormClosing);
            this.Load += new System.EventHandler(this.CopyFiles_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxSourceFolders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butAddSourceFolder;
        private System.Windows.Forms.CheckedListBox checkedListBoxDestFolders;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butCopy;
        private System.Windows.Forms.Label label2;
    }
}