namespace ZiPreview
{
    partial class VeracryptCreateDialog
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
            this.txtVolume = new System.Windows.Forms.TextBox();
            this.butSelectFile = new System.Windows.Forms.Button();
            this.lblVolume = new System.Windows.Forms.Label();
            this.butCreate = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.cboSizeUnits = new System.Windows.Forms.ComboBox();
            this.lblSize = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtVolume
            // 
            this.txtVolume.Location = new System.Drawing.Point(61, 24);
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.Size = new System.Drawing.Size(376, 20);
            this.txtVolume.TabIndex = 0;
            this.txtVolume.TextChanged += new System.EventHandler(this.TxtVolume_TextChanged);
            // 
            // butSelectFile
            // 
            this.butSelectFile.Location = new System.Drawing.Point(443, 25);
            this.butSelectFile.Name = "butSelectFile";
            this.butSelectFile.Size = new System.Drawing.Size(29, 19);
            this.butSelectFile.TabIndex = 1;
            this.butSelectFile.Text = "...";
            this.butSelectFile.UseVisualStyleBackColor = true;
            this.butSelectFile.Click += new System.EventHandler(this.ButSelectFile_Click);
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Location = new System.Drawing.Point(10, 25);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(45, 13);
            this.lblVolume.TabIndex = 2;
            this.lblVolume.Text = "Volume:";
            // 
            // butCreate
            // 
            this.butCreate.Location = new System.Drawing.Point(13, 138);
            this.butCreate.Name = "butCreate";
            this.butCreate.Size = new System.Drawing.Size(75, 28);
            this.butCreate.TabIndex = 3;
            this.butCreate.Text = "Create";
            this.butCreate.UseVisualStyleBackColor = true;
            this.butCreate.Click += new System.EventHandler(this.ButCreate_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(393, 142);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(78, 24);
            this.butCancel.TabIndex = 4;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.ButCancel_Click);
            // 
            // cboSizeUnits
            // 
            this.cboSizeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSizeUnits.FormattingEnabled = true;
            this.cboSizeUnits.Location = new System.Drawing.Point(140, 59);
            this.cboSizeUnits.Name = "cboSizeUnits";
            this.cboSizeUnits.Size = new System.Drawing.Size(51, 21);
            this.cboSizeUnits.TabIndex = 5;
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(19, 63);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(30, 13);
            this.lblSize.TabIndex = 6;
            this.lblSize.Text = "Size:";
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(61, 59);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(67, 20);
            this.txtSize.TabIndex = 7;
            this.txtSize.TextChanged += new System.EventHandler(this.TxtSize_TextChanged);
            this.txtSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtSize_KeyPress);
            // 
            // VeracryptDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 178);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.cboSizeUnits);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butCreate);
            this.Controls.Add(this.lblVolume);
            this.Controls.Add(this.butSelectFile);
            this.Controls.Add(this.txtVolume);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VeracryptDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "VeracryptDialog";
            this.Load += new System.EventHandler(this.VeracryptDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtVolume;
        private System.Windows.Forms.Button butSelectFile;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.Button butCreate;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.ComboBox cboSizeUnits;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.TextBox txtSize;
    }
}