using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using ModulesLoader.Classes;
using ModulesLoader.Data;
using ModulesLoader.Properties;
using Xceed.Compression;
using PUSPolises;

namespace ModulesLoader
{
    internal class Program
    {

        #region Local Variables

        private static FrmProgressClass _frmProgress;
        private static string _strLoaderName;
        private static string _strLoaderVersion;

        // PUSPolises
        //public static frmLoginClass frmLogin;
        //public static frmMainClass frmMain;
        //public static frmAboutClass frmAbout;

        #endregion

        [STAThread]
        private static void Main()
        {
            try
            {
                if (MyClasses.RunningInstance() != null) return;

                Licenser.LicenseKey = "ZIN37-U8JE5-P55Z8-YKCA";
                var objEntry = Dns.GetHostEntry(Dns.GetHostName());
                MyClasses._strHostName = objEntry.HostName;
                try
                {
                    MyClasses._strHostIp = objEntry.AddressList.Where(oneIP => oneIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray()[0].ToString();
                }
                catch (Exception)
                {
                    MyClasses._strHostIp = "192.168.";
                }
                var insAssembly = Assembly.GetExecutingAssembly();
                //_strLoaderVersion = insAssembly.GetName().Version.ToString();
                //_strLoaderName = insAssembly.GetName().Name;
                MyClasses._intProjectId = Settings.Default.ProjectID;
                //#if DEBUG 
                //                //MyClasses._intProjectId = 99;  
                //#endif

                if (MyClasses._strHostIp.StartsWith("192.168."))
                {
                    MyClasses._strServerName = Settings.Default.ServerName;
                }
                else
                {
                    MyClasses._strServerName = Settings.Default.ServerIP;
                }
                MyClasses._strServerName = Settings.Default.ServerName;

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return;
            }
            CheckNewVersions();
        }

        private static void CheckNewVersions()
        {

            try
            {
                string strConnection = string.Format(MyClasses._strConnection, MyClasses._strServerName);

                var versionDb = new VersionDBDataContext(strConnection);

                MyClasses.LoadNewVersions(MyClasses._strHostName, MyClasses._strHostIp);

                PUSUpdate insPUSUpdate = new PUSUpdate();
                Thread PUSUpdateThread = new Thread(insPUSUpdate.StartTestForUpdates);

                PUSWorker insPUSWorker = new PUSWorker();
                Thread PUSWorkerThread = new Thread(insPUSWorker.StartPUS);

                // Start Test For Update thread.
                PUSUpdateThread.Start();

                // Start PUS.
                PUSWorkerThread.Start();

                versionDb.Dispose();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }
    }
}