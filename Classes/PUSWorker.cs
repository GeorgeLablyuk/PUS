﻿using ModulesLoader.Data;
using ModulesLoader.Properties;
using PUSPolises;
using System.Threading;
using System.Windows.Forms;

namespace ModulesLoader.Classes
{
    public class PUSUpdate
    {
        
        static frmPUSUpdate frmUpdates = new frmPUSUpdate();

        internal void StartTestForUpdates()
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
            MyClasses.LoadBatchHandler();
            MyClasses.ShellNoWait(Settings.Default.BatchHandlerName);
            Application.Exit();
        }
    }

    public class PUSWorker
    {
        internal static void StartPUS()
        {
            StartPUS insStartPUS = new StartPUS();
            insStartPUS.PusRun();
            MyClasses._shouldStop = true;
        }
    }
}
