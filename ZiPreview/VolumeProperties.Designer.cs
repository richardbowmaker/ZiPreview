namespace ZiPreview
{
    partial class VolumeProperties
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
            this.txtFolder1 = new System.Windows.Forms.TextBox();
            this.butFolder1 = new System.Windows.Forms.Button();
            this.lstVolumes = new System.Windows.Forms.ListBox();
            this.butFolder2 = new System.Windows.Forms.Button();
            this.txtFolder2 = new System.Windows.Forms.TextBox();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.butFind = new System.Windows.Forms.Button();
            this.txtCreated1 = new System.Windows.Forms.TextBox();
            this.txtModified1 = new System.Windows.Forms.TextBox();
            this.txtAccessed1 = new System.Windows.Forms.TextBox();
            this.txtAccessed2 = new System.Windows.Forms.TextBox();
            this.txtModified2 = new System.Windows.Forms.TextBox();
            this.txtCreated2 = new System.Windows.Forms.TextBox();
            this.datePicker = new System.Windows.Forms.DateTimePicker();
            this.butSet = new System.Windows.Forms.Button();
            this.timePicker = new System.Windows.Forms.DateTimePicker();
            this.butReset = new System.Windows.Forms.Button();
            this.butCopyDates = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtFolder1
            // 
            this.txtFolder1.Location = new System.Drawing.Point(14, 15);
            this.txtFolder1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFolder1.Name = "txtFolder1";
            this.txtFolder1.ReadOnly = true;
            this.txtFolder1.Size = new System.Drawing.Size(349, 26);
            this.txtFolder1.TabIndex = 0;
            // 
            // butFolder1
            // 
            this.butFolder1.Location = new System.Drawing.Point(390, 14);
            this.butFolder1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butFolder1.Name = "butFolder1";
            this.butFolder1.Size = new System.Drawing.Size(43, 29);
            this.butFolder1.TabIndex = 1;
            this.butFolder1.Text = "...";
            this.butFolder1.UseVisualStyleBackColor = true;
            this.butFolder1.Click += new System.EventHandler(this.butFolder1_Click);
            // 
            // lstVolumes
            // 
            this.lstVolumes.FormattingEnabled = true;
            this.lstVolumes.ItemHeight = 20;
            this.lstVolumes.Location = new System.Drawing.Point(14, 182);
            this.lstVolumes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lstVolumes.Name = "lstVolumes";
            this.lstVolumes.Size = new System.Drawing.Size(886, 364);
            this.lstVolumes.TabIndex = 2;
            this.lstVolumes.SelectedIndexChanged += new System.EventHandler(this.lstVolumes_SelectedIndexChanged);
            // 
            // butFolder2
            // 
            this.butFolder2.Location = new System.Drawing.Point(390, 66);
            this.butFolder2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butFolder2.Name = "butFolder2";
            this.butFolder2.Size = new System.Drawing.Size(43, 29);
            this.butFolder2.TabIndex = 4;
            this.butFolder2.Text = "...";
            this.butFolder2.UseVisualStyleBackColor = true;
            this.butFolder2.Click += new System.EventHandler(this.butFolder2_Click);
            // 
            // txtFolder2
            // 
            this.txtFolder2.Location = new System.Drawing.Point(14, 68);
            this.txtFolder2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFolder2.Name = "txtFolder2";
            this.txtFolder2.ReadOnly = true;
            this.txtFolder2.Size = new System.Drawing.Size(349, 26);
            this.txtFolder2.TabIndex = 3;
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(14, 128);
            this.txtFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(216, 26);
            this.txtFilter.TabIndex = 5;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // butFind
            // 
            this.butFind.Location = new System.Drawing.Point(266, 129);
            this.butFind.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butFind.Name = "butFind";
            this.butFind.Size = new System.Drawing.Size(88, 46);
            this.butFind.TabIndex = 6;
            this.butFind.Text = "Find";
            this.butFind.UseVisualStyleBackColor = true;
            this.butFind.Click += new System.EventHandler(this.butFind_Click);
            // 
            // txtCreated1
            // 
            this.txtCreated1.Location = new System.Drawing.Point(932, 15);
            this.txtCreated1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCreated1.Name = "txtCreated1";
            this.txtCreated1.ReadOnly = true;
            this.txtCreated1.Size = new System.Drawing.Size(223, 26);
            this.txtCreated1.TabIndex = 8;
            this.txtCreated1.Click += new System.EventHandler(this.txtCreated1_Click);
            // 
            // txtModified1
            // 
            this.txtModified1.Location = new System.Drawing.Point(932, 71);
            this.txtModified1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtModified1.Name = "txtModified1";
            this.txtModified1.ReadOnly = true;
            this.txtModified1.Size = new System.Drawing.Size(220, 26);
            this.txtModified1.TabIndex = 9;
            this.txtModified1.Click += new System.EventHandler(this.txtModified1_Click);
            // 
            // txtAccessed1
            // 
            this.txtAccessed1.Location = new System.Drawing.Point(932, 121);
            this.txtAccessed1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAccessed1.Name = "txtAccessed1";
            this.txtAccessed1.ReadOnly = true;
            this.txtAccessed1.Size = new System.Drawing.Size(220, 26);
            this.txtAccessed1.TabIndex = 10;
            this.txtAccessed1.Click += new System.EventHandler(this.txtAccessed1_Click);
            // 
            // txtAccessed2
            // 
            this.txtAccessed2.Location = new System.Drawing.Point(932, 292);
            this.txtAccessed2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAccessed2.Name = "txtAccessed2";
            this.txtAccessed2.ReadOnly = true;
            this.txtAccessed2.Size = new System.Drawing.Size(220, 26);
            this.txtAccessed2.TabIndex = 13;
            this.txtAccessed2.Click += new System.EventHandler(this.txtAccessed2_Click);
            // 
            // txtModified2
            // 
            this.txtModified2.Location = new System.Drawing.Point(932, 242);
            this.txtModified2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtModified2.Name = "txtModified2";
            this.txtModified2.ReadOnly = true;
            this.txtModified2.Size = new System.Drawing.Size(220, 26);
            this.txtModified2.TabIndex = 12;
            this.txtModified2.Click += new System.EventHandler(this.txtModified2_Click);
            // 
            // txtCreated2
            // 
            this.txtCreated2.Location = new System.Drawing.Point(932, 194);
            this.txtCreated2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCreated2.Name = "txtCreated2";
            this.txtCreated2.ReadOnly = true;
            this.txtCreated2.Size = new System.Drawing.Size(220, 26);
            this.txtCreated2.TabIndex = 11;
            this.txtCreated2.Click += new System.EventHandler(this.txtCreated2_Click);
            // 
            // datePicker
            // 
            this.datePicker.Location = new System.Drawing.Point(924, 381);
            this.datePicker.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(228, 26);
            this.datePicker.TabIndex = 14;
            // 
            // butSet
            // 
            this.butSet.Location = new System.Drawing.Point(1092, 495);
            this.butSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butSet.Name = "butSet";
            this.butSet.Size = new System.Drawing.Size(60, 44);
            this.butSet.TabIndex = 15;
            this.butSet.Text = "Set";
            this.butSet.UseVisualStyleBackColor = true;
            this.butSet.Click += new System.EventHandler(this.butSet_Click);
            // 
            // timePicker
            // 
            this.timePicker.Location = new System.Drawing.Point(924, 440);
            this.timePicker.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.timePicker.Name = "timePicker";
            this.timePicker.Size = new System.Drawing.Size(224, 26);
            this.timePicker.TabIndex = 16;
            // 
            // butReset
            // 
            this.butReset.Location = new System.Drawing.Point(377, 126);
            this.butReset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.butReset.Name = "butReset";
            this.butReset.Size = new System.Drawing.Size(101, 49);
            this.butReset.TabIndex = 17;
            this.butReset.Text = "Reset";
            this.butReset.UseVisualStyleBackColor = true;
            this.butReset.Click += new System.EventHandler(this.butReset_Click);
            // 
            // butCopyDates
            // 
            this.butCopyDates.Location = new System.Drawing.Point(556, 129);
            this.butCopyDates.Name = "butCopyDates";
            this.butCopyDates.Size = new System.Drawing.Size(143, 45);
            this.butCopyDates.TabIndex = 18;
            this.butCopyDates.Text = "Copy Dates";
            this.butCopyDates.UseVisualStyleBackColor = true;
            this.butCopyDates.Click += new System.EventHandler(this.butCopyDates_Click);
            // 
            // VolumeProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 562);
            this.Controls.Add(this.butCopyDates);
            this.Controls.Add(this.butReset);
            this.Controls.Add(this.timePicker);
            this.Controls.Add(this.butSet);
            this.Controls.Add(this.datePicker);
            this.Controls.Add(this.txtAccessed2);
            this.Controls.Add(this.txtModified2);
            this.Controls.Add(this.txtCreated2);
            this.Controls.Add(this.txtAccessed1);
            this.Controls.Add(this.txtModified1);
            this.Controls.Add(this.txtCreated1);
            this.Controls.Add(this.butFind);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.butFolder2);
            this.Controls.Add(this.txtFolder2);
            this.Controls.Add(this.lstVolumes);
            this.Controls.Add(this.butFolder1);
            this.Controls.Add(this.txtFolder1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VolumeProperties";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "VolumeProperties";
            this.Load += new System.EventHandler(this.VolumeProperties_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFolder1;
        private System.Windows.Forms.Button butFolder1;
        private System.Windows.Forms.ListBox lstVolumes;
        private System.Windows.Forms.Button butFolder2;
        private System.Windows.Forms.TextBox txtFolder2;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Button butFind;
        private System.Windows.Forms.TextBox txtCreated1;
        private System.Windows.Forms.TextBox txtModified1;
        private System.Windows.Forms.TextBox txtAccessed1;
        private System.Windows.Forms.TextBox txtAccessed2;
        private System.Windows.Forms.TextBox txtModified2;
        private System.Windows.Forms.TextBox txtCreated2;
        private System.Windows.Forms.DateTimePicker datePicker;
        private System.Windows.Forms.Button butSet;
        private System.Windows.Forms.DateTimePicker timePicker;
        private System.Windows.Forms.Button butReset;
        private System.Windows.Forms.Button butCopyDates;
    }
}