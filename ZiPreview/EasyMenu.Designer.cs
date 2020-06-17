namespace ZiPreview
{
    partial class EasyMenu
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
            this.components = new System.ComponentModel.Container();
            this.lstOptions = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstOptions
            // 
            this.lstOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstOptions.FormattingEnabled = true;
            this.lstOptions.ItemHeight = 20;
            this.lstOptions.Location = new System.Drawing.Point(0, 0);
            this.lstOptions.Name = "lstOptions";
            this.lstOptions.Size = new System.Drawing.Size(736, 880);
            this.lstOptions.TabIndex = 0;
            this.lstOptions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LstOptions_KeyDown);
            this.lstOptions.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LstOptions_KeyUp);
            // 
            // EasyMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 880);
            this.Controls.Add(this.lstOptions);
            this.Name = "EasyMenu";
            this.Text = "EasyMenu";
            this.Load += new System.EventHandler(this.EasyMenu_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EasyMenu_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EasyMenu_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstOptions;
    }
}