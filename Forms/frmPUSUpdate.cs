using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ModulesLoader.Classes;

namespace ModulesLoader
{
    public partial class frmPUSUpdate : Form
    {
        int intCounter = 30;

        public frmPUSUpdate()
        {
            InitializeComponent();
        }


        private void timeCounter_Tick(object sender, EventArgs e)
        {
            this.lblTime.Text = intCounter.ToString();
            intCounter -= 1;
            this.Refresh();
            if (intCounter == 0) this.Close();
        }

        private void frmPUSUpdate_Load(object sender, EventArgs e)
        {
            this.Text = "P.U.S. Sistēma atjaunošāna!!!";
            this.lblFromToVersion.Text = "V." + MyClasses._strPUSOldVersion + " -> " + "V." + MyClasses._strPUSOldVersion;
            this.timeCounter.Interval = (1000) * (1);              // Timer will tick evert second
            this.timeCounter.Enabled = true;                       // Enable the timer
            this.timeCounter.Start();
            
        }

        private void btnLabi_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
