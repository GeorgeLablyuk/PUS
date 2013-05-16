using ModulesLoader.Properties;
using ModulesLoader.Data;
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;


namespace ModulesLoader.Classes
{
    internal class PUSWorker
    {
        private static bool _shouldStop;
        static frmPUSUpdate frmUpdates = new frmPUSUpdate();

        public static void StartTestForUpdates()
        {
            string strConnection = string.Format(MyClasses._strConnection, MyClasses._strServerName);
            var versionDb = new VersionDBDataContext(strConnection);
            _shouldStop = false;

            while (!_shouldStop)
            {
                if (IsPUSWorked() == null) break;
                Thread.Sleep(Settings.Default.WaitForTest); // Waitin in XX second 

                int intUpdateNumber = 0;
                int intNewNeedUpdate = versionDb.tUpdateNumbers.Single(
                    one => one.AssemblyProjectID == MyClasses._intProjectId).UpdateNumber;


                if (!File.Exists(MyClasses._strNeedLoadUpdateFileName) ||
                    int.TryParse(File.ReadAllText(MyClasses._strNeedLoadUpdateFileName), out intUpdateNumber) &&
                    intUpdateNumber < intNewNeedUpdate)
                {
                    frmUpdates.Show();
                    Application.DoEvents();
                    //frmUpdates.Refresh();
                    Thread.Sleep(Settings.Default.WaitForEnd);
                    
                    frmUpdates.Hide();
                   
                    if (IsPUSWorked() != null)
                    {
                        IsPUSWorked().Kill();
                        Thread.Sleep(100);
                    }
                    MyClasses._blnPUSIsKilled = true;
                    _shouldStop = true;
                }

            }
        }

        public static Process IsPUSWorked()
        {
            List<Process> prcArray = Process.GetProcesses().ToList();
            return prcArray.Find(pus => pus.ProcessName.Equals(MyClasses._strExecutableName.Replace(".exe", "")));
        }
 
    }
}
