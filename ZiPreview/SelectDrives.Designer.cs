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
            this.txtEnter = new System.Windows.Forms.TextBox();
            this.butFind = new System.Windows.Forms.Button();
            this.butSelectAll = new System.Windows.Forms.Button();
            this.butDeselectAll = new System.Windows.Forms.Button();
            this.nudSelect = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(35, 34);
            this.txtFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(771, 26);
            this.txtFolder.TabIndex = 0;
            this.txtFolder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFolder_KeyPress);
            // 
            // butFolder
            // 
            this.butFolder.Location = new System.Drawing.Point(843, 35);
            this.butFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butFolder.Name = "butFolder";
            this.butFolder.Size = new System.Drawing.Size(35, 29);
            this.butFolder.TabIndex = 1;
            this.butFolder.Text = "...";
            this.butFolder.UseVisualStyleBackColor = true;
            this.butFolder.Click += new System.EventHandler(this.butFolder_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(35, 81);
            this.txtFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(249, 26);
            this.txtFilter.TabIndex = 2;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            this.txtFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilter_KeyPress);
            // 
            // chkFiles
            // 
            this.chkFiles.CheckOnClick = true;
            this.chkFiles.FormattingEnabled = true;
            this.chkFiles.Location = new System.Drawing.Point(35, 135);
            this.chkFiles.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkFiles.Name = "chkFiles";
            this.chkFiles.Size = new System.Drawing.Size(911, 464);
            this.chkFiles.TabIndex = 3;
            // 
            // butOk
            // 
            this.butOk.Location = new System.Drawing.Point(35, 706);
            this.butOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butOk.Name = "butOk";
            this.butOk.Size = new System.Drawing.Size(100, 35);
            this.butOk.TabIndex = 4;
            this.butOk.Text = "OK";
            this.butOk.UseVisualStyleBackColor = true;
            this.butOk.Click += new System.EventHandler(this.butOk_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(994, 706);
            this.butCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(100, 35);
            this.butCancel.TabIndex = 5;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // txtEnter
            // 
            this.txtEnter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEnter.Location = new System.Drawing.Point(35, 628);
            this.txtEnter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEnter.Name = "txtEnter";
            this.txtEnter.Size = new System.Drawing.Size(381, 35);
            this.txtEnter.TabIndex = 7;
            this.txtEnter.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            this.txtEnter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            // 
            // butFind
            // 
            this.butFind.Location = new System.Drawing.Point(970, 135);
            this.butFind.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butFind.Name = "butFind";
            this.butFind.Size = new System.Drawing.Size(84, 49);
            this.butFind.TabIndex = 8;
            this.butFind.Text = "Find";
            this.butFind.UseVisualStyleBackColor = true;
            this.butFind.Click += new System.EventHandler(this.butFind_Click);
            // 
            // butSelectAll
            // 
            this.butSelectAll.Location = new System.Drawing.Point(970, 226);
            this.butSelectAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butSelectAll.Name = "butSelectAll";
            this.butSelectAll.Size = new System.Drawing.Size(128, 46);
            this.butSelectAll.TabIndex = 9;
            this.butSelectAll.Text = "Select All";
            this.butSelectAll.UseVisualStyleBackColor = true;
            this.butSelectAll.Click += new System.EventHandler(this.butSelectAll_Click);
            // 
            // butDeselectAll
            // 
            this.butDeselectAll.Location = new System.Drawing.Point(970, 311);
            this.butDeselectAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butDeselectAll.Name = "butDeselectAll";
            this.butDeselectAll.Size = new System.Drawing.Size(125, 46);
            this.butDeselectAll.TabIndex = 10;
            this.butDeselectAll.Text = "Deselect All";
            this.butDeselectAll.UseVisualStyleBackColor = true;
            this.butDeselectAll.Click += new System.EventHandler(this.butDeselectAll_Click);
            // 
            // nudSelect
            // 
            this.nudSelect.Location = new System.Drawing.Point(994, 444);
            this.nudSelect.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudSelect.Name = "nudSelect";
            this.nudSelect.Size = new System.Drawing.Size(56, 26);
            this.nudSelect.TabIndex = 11;
            this.nudSelect.ValueChanged += new System.EventHandler(this.nudSelect_ValueChanged);
            // 
            // SelectDrives
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 756);
            this.Controls.Add(this.nudSelect);
            this.Controls.Add(this.butDeselectAll);
            this.Controls.Add(this.butSelectAll);
            this.Controls.Add(this.butFind);
            this.Controls.Add(this.txtEnter);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOk);
            this.Controls.Add(this.chkFiles);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.butFolder);
            this.Controls.Add(this.txtFolder);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectDrives";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SelectDrives";
            this.Load += new System.EventHandler(this.SelectDrives_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudSelect)).EndInit();
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
        private System.Windows.Forms.TextBox txtEnter;
        private System.Windows.Forms.Button butFind;
        private System.Windows.Forms.Button butSelectAll;
        private System.Windows.Forms.Button butDeselectAll;
        private System.Windows.Forms.NumericUpDown nudSelect;
    }
}