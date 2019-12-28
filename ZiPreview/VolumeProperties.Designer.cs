﻿namespace ZiPreview
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
            this.SuspendLayout();
            // 
            // txtFolder1
            // 
            this.txtFolder1.Location = new System.Drawing.Point(12, 12);
            this.txtFolder1.Name = "txtFolder1";
            this.txtFolder1.ReadOnly = true;
            this.txtFolder1.Size = new System.Drawing.Size(311, 22);
            this.txtFolder1.TabIndex = 0;
            // 
            // butFolder1
            // 
            this.butFolder1.Location = new System.Drawing.Point(347, 11);
            this.butFolder1.Name = "butFolder1";
            this.butFolder1.Size = new System.Drawing.Size(38, 23);
            this.butFolder1.TabIndex = 1;
            this.butFolder1.Text = "...";
            this.butFolder1.UseVisualStyleBackColor = true;
            this.butFolder1.Click += new System.EventHandler(this.butFolder1_Click);
            // 
            // lstVolumes
            // 
            this.lstVolumes.FormattingEnabled = true;
            this.lstVolumes.ItemHeight = 16;
            this.lstVolumes.Location = new System.Drawing.Point(12, 146);
            this.lstVolumes.Name = "lstVolumes";
            this.lstVolumes.Size = new System.Drawing.Size(788, 292);
            this.lstVolumes.TabIndex = 2;
            this.lstVolumes.SelectedIndexChanged += new System.EventHandler(this.lstVolumes_SelectedIndexChanged);
            // 
            // butFolder2
            // 
            this.butFolder2.Location = new System.Drawing.Point(347, 53);
            this.butFolder2.Name = "butFolder2";
            this.butFolder2.Size = new System.Drawing.Size(38, 23);
            this.butFolder2.TabIndex = 4;
            this.butFolder2.Text = "...";
            this.butFolder2.UseVisualStyleBackColor = true;
            this.butFolder2.Click += new System.EventHandler(this.butFolder2_Click);
            // 
            // txtFolder2
            // 
            this.txtFolder2.Location = new System.Drawing.Point(12, 54);
            this.txtFolder2.Name = "txtFolder2";
            this.txtFolder2.ReadOnly = true;
            this.txtFolder2.Size = new System.Drawing.Size(311, 22);
            this.txtFolder2.TabIndex = 3;
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(12, 102);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(192, 22);
            this.txtFilter.TabIndex = 5;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // butFind
            // 
            this.butFind.Location = new System.Drawing.Point(236, 103);
            this.butFind.Name = "butFind";
            this.butFind.Size = new System.Drawing.Size(78, 37);
            this.butFind.TabIndex = 6;
            this.butFind.Text = "Find";
            this.butFind.UseVisualStyleBackColor = true;
            this.butFind.Click += new System.EventHandler(this.butFind_Click);
            // 
            // txtCreated1
            // 
            this.txtCreated1.Location = new System.Drawing.Point(828, 12);
            this.txtCreated1.Name = "txtCreated1";
            this.txtCreated1.ReadOnly = true;
            this.txtCreated1.Size = new System.Drawing.Size(199, 22);
            this.txtCreated1.TabIndex = 8;
            this.txtCreated1.Click += new System.EventHandler(this.txtCreated1_Click);
            // 
            // txtModified1
            // 
            this.txtModified1.Location = new System.Drawing.Point(828, 57);
            this.txtModified1.Name = "txtModified1";
            this.txtModified1.ReadOnly = true;
            this.txtModified1.Size = new System.Drawing.Size(196, 22);
            this.txtModified1.TabIndex = 9;
            this.txtModified1.Click += new System.EventHandler(this.txtModified1_Click);
            // 
            // txtAccessed1
            // 
            this.txtAccessed1.Location = new System.Drawing.Point(828, 97);
            this.txtAccessed1.Name = "txtAccessed1";
            this.txtAccessed1.ReadOnly = true;
            this.txtAccessed1.Size = new System.Drawing.Size(196, 22);
            this.txtAccessed1.TabIndex = 10;
            this.txtAccessed1.Click += new System.EventHandler(this.txtAccessed1_Click);
            // 
            // txtAccessed2
            // 
            this.txtAccessed2.Location = new System.Drawing.Point(828, 234);
            this.txtAccessed2.Name = "txtAccessed2";
            this.txtAccessed2.ReadOnly = true;
            this.txtAccessed2.Size = new System.Drawing.Size(196, 22);
            this.txtAccessed2.TabIndex = 13;
            this.txtAccessed2.Click += new System.EventHandler(this.txtAccessed2_Click);
            // 
            // txtModified2
            // 
            this.txtModified2.Location = new System.Drawing.Point(828, 194);
            this.txtModified2.Name = "txtModified2";
            this.txtModified2.ReadOnly = true;
            this.txtModified2.Size = new System.Drawing.Size(196, 22);
            this.txtModified2.TabIndex = 12;
            this.txtModified2.Click += new System.EventHandler(this.txtModified2_Click);
            // 
            // txtCreated2
            // 
            this.txtCreated2.Location = new System.Drawing.Point(828, 155);
            this.txtCreated2.Name = "txtCreated2";
            this.txtCreated2.ReadOnly = true;
            this.txtCreated2.Size = new System.Drawing.Size(196, 22);
            this.txtCreated2.TabIndex = 11;
            this.txtCreated2.Click += new System.EventHandler(this.txtCreated2_Click);
            // 
            // datePicker
            // 
            this.datePicker.Location = new System.Drawing.Point(821, 305);
            this.datePicker.Name = "datePicker";
            this.datePicker.Size = new System.Drawing.Size(203, 22);
            this.datePicker.TabIndex = 14;
            // 
            // butSet
            // 
            this.butSet.Location = new System.Drawing.Point(971, 396);
            this.butSet.Name = "butSet";
            this.butSet.Size = new System.Drawing.Size(53, 35);
            this.butSet.TabIndex = 15;
            this.butSet.Text = "Set";
            this.butSet.UseVisualStyleBackColor = true;
            this.butSet.Click += new System.EventHandler(this.butSet_Click);
            // 
            // timePicker
            // 
            this.timePicker.Location = new System.Drawing.Point(821, 352);
            this.timePicker.Name = "timePicker";
            this.timePicker.Size = new System.Drawing.Size(200, 22);
            this.timePicker.TabIndex = 16;
            // 
            // VolumeProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 450);
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
    }
}