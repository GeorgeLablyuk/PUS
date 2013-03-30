namespace ModulesLoader
{
    partial class FrmProgressClass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProgressClass));
            this.pcxPicture = new System.Windows.Forms.PictureBox();
            this.pbrProgressBar = new System.Windows.Forms.ProgressBar();
            this.lblAssemblyName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pcxPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // pcxPicture
            // 
            this.pcxPicture.Image = ((System.Drawing.Image)(resources.GetObject("pcxPicture.Image")));
            this.pcxPicture.Location = new System.Drawing.Point(147, 15);
            this.pcxPicture.Name = "pcxPicture";
            this.pcxPicture.Size = new System.Drawing.Size(107, 35);
            this.pcxPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pcxPicture.TabIndex = 22;
            this.pcxPicture.TabStop = false;
            // 
            // pbrProgressBar
            // 
            this.pbrProgressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbrProgressBar.Location = new System.Drawing.Point(0, 77);
            this.pbrProgressBar.Name = "pbrProgressBar";
            this.pbrProgressBar.Size = new System.Drawing.Size(407, 23);
            this.pbrProgressBar.TabIndex = 23;
            // 
            // lblAssemblyName
            // 
            this.lblAssemblyName.AutoSize = true;
            this.lblAssemblyName.Location = new System.Drawing.Point(63, 81);
            this.lblAssemblyName.Name = "lblAssemblyName";
            this.lblAssemblyName.Size = new System.Drawing.Size(10, 13);
            this.lblAssemblyName.TabIndex = 24;
            this.lblAssemblyName.Text = ".";
            // 
            // frmProgressClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 100);
            this.Controls.Add(this.lblAssemblyName);
            this.Controls.Add(this.pbrProgressBar);
            this.Controls.Add(this.pcxPicture);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProgressClass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get  versions from server";
            ((System.ComponentModel.ISupportInitialize)(this.pcxPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.PictureBox pcxPicture;
        public System.Windows.Forms.ProgressBar pbrProgressBar;
        public System.Windows.Forms.Label lblAssemblyName;
    }
}