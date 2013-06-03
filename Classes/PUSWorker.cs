using ModulesLoader.Properties;
using ModulesLoader.Data;
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using PUSPolises;

namespace ModulesLoader.Classes
{
    public class PUSUpdate
    {
        
        static frmPUSUpdate frmUpdates = new frmPUSUpdate();

        public  void StartTestForUpdates()
        {
            string strConnection = string.Format(MyClasses._strConnection, MyClasses._strServerName);
            var versionDb = new VersionDBDataContext(strConnection);
            MyClasses._shouldStop = false;

            while (!MyClasses._shouldStop)
            {
                Thread.Sleep(Settings.Default.WaitForTest); // Waitin in XX second 

                if (MyClasses.LoadNewVersions(MyClasses._strHostName, MyClasses._strHostIp))
                {
                    frmUpdates.Show();
                    Application.DoEvents();
                    frmUpdates.Refresh();
                    Thread.Sleep(Settings.Default.WaitForEnd);
                    
                    frmUpdates.Hide();
                    MyClasses._shouldStop = true;
                }

            }
        }
    }

    public class PUSWorker
    {
        internal  void StartPUS()
        {
            StartPUS insStartPUS = new StartPUS();
            insStartPUS.PusRun();
            MyClasses._shouldStop = true;
        }
    }
}
