using ModulesLoader.Properties;
using ModulesLoader.Data;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


namespace ModulesLoader.Classes
{
    class PUSWorker
    {
        public void StartTestForUpdates()
        {
            string strConnection = string.Format(MyClasses._strConnection, MyClasses._strServerName);
            var versionDb = new VersionDBDataContext(strConnection);

            while (!_shouldStop)
            {
                // Put the main thread to sleep for 1 millisecond to
                // allow the worker thread to do some work:
                Thread.Sleep(Settings.Default.WaitForTest); // Waitin in second 

                int intUpdateNumber = 0;
                int intNewNeedUpdate = versionDb.tUpdateNumbers.Single(
                    one => one.AssemblyProjectID == MyClasses._intProjectId).UpdateNumber;

                if (!File.Exists(MyClasses._strNeedLoadUpdateFileName) ||
                    int.TryParse(File.ReadAllText(MyClasses._strNeedLoadUpdateFileName), out intUpdateNumber) &&
                    intUpdateNumber < intNewNeedUpdate)
                {
                    DialogResult dlgWhat = MessageBox.Show("Exist new version of P.U.S. Please close and restart P.U.S", "P.U.S. Updater",MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (dlgWhat.Equals(DialogResult.Yes))
                    {
                        //MyClasses.PUSUpdatesTestObject.RequestStop();
                        _shouldStop = true;
                    }
                }

            }
        }
        public void RequestStop()
        {
            _shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data
        // member will be accessed by multiple threads.
        private volatile bool _shouldStop;
    }
}
