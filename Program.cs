using ModulesLoader.Classes;
using ModulesLoader.Data;
using ModulesLoader.Properties;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;

namespace ModulesLoader
{
    internal class Program
    {

        [STAThread]
        private static void Main()
        {
            try
            {
                if (MyClasses.RunningInstance() != null) return;

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

                MyClasses._intProjectId = Settings.Default.ProjectID;

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
                MyClasses._strConnection = string.Format(MyClasses._strConnection, MyClasses._strServerName);

                var versionDb = new VersionDBDataContext(MyClasses._strConnection);
                if (File.Exists(Settings.Default.BatchHandlerName))
                {
                    File.Delete(Settings.Default.BatchHandlerName);
                }

                PUSUpdate insPUSUpdate = new PUSUpdate();
                Thread PUSUpdateThread = new Thread(insPUSUpdate.StartTestForUpdates);
               
                // Start Test For Update thread.
                PUSUpdateThread.Start();
                // Start PUS.
                PUSWorker.StartPUS();

                versionDb.Dispose();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }

    }
}