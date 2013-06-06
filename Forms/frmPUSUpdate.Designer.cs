namespace ModulesLoader
{
    partial class frmPUSUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPUSUpdate));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timeCounter = new System.Windows.Forms.Timer(this.components);
            this.lblTime = new System.Windows.Forms.Label();
            this.btnLabi = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblFromToVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(12, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(306, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ir pieejama jauna PUS versija.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(1, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(325, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Sistēma atjaunosies automātiski";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel1.Location = new System.Drawing.Point(19, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 102);
            this.panel1.TabIndex = 2;
            // 
            // timeCounter
            // 
            this.timeCounter.Interval = 1000;
            this.timeCounter.Tick += new System.EventHandler(this.timeCounter_Tick);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblTime.ForeColor = System.Drawing.Color.DarkRed;
            this.lblTime.Location = new System.Drawing.Point(47, 187);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(57, 39);
            this.lblTime.TabIndex = 3;
            this.lblTime.Text = "30";
            // 
            // btnLabi
            // 
            this.btnLabi.Location = new System.Drawing.Point(232, 196);
            this.btnLabi.Name = "btnLabi";
            this.btnLabi.Size = new System.Drawing.Size(75, 23);
            this.btnLabi.TabIndex = 4;
            this.btnLabi.Text = "&Labi";
            this.btnLabi.UseVisualStyleBackColor = true;
            this.btnLabi.Click += new System.EventHandler(this.btnLabi_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.label3.ForeColor = System.Drawing.Color.Maroon;
            this.label3.Location = new System.Drawing.Point(2, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(220, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "pēc           sekundēm.";
            // 
            // lblFromToVersion
            // 
            this.lblFromToVersion.AutoSize = true;
            this.lblFromToVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblFromToVersion.ForeColor = System.Drawing.Color.Green;
            this.lblFromToVersion.Location = new System.Drawing.Point(86, 112);
            this.lblFromToVersion.Name = "lblFromToVersion";
            this.lblFromToVersion.Size = new System.Drawing.Size(155, 20);
            this.lblFromToVersion.TabIndex = 6;
            this.lblFromToVersion.Text = "3.5.1.3 -> 3.5.1.12";
            // 
            // frmPUSUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(207)))), ((int)(((byte)(152)))));
            this.ClientSize = new System.Drawing.Size(327, 225);
            this.ControlBox = false;
            this.Controls.Add(this.lblFromToVersion);
            this.Controls.Add(this.btnLabi);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPUSUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "P.U.S. Sistēma atjaunošāna!!!";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPUSUpdate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timeCounter;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Button btnLabi;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblFromToVersion;
    }
}