using ModulesLoader.Data;
using ModulesLoader.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Xceed.Compression;


namespace ModulesLoader.Classes
{
    internal class MyClasses
    {

        #region Static Variables

        internal static string _strHostIp;
        internal static string _strHostName;

        internal static string _strServerName;
        internal static string _strExecutableName;
        internal static string _strExecutableNewName;

        internal static string _strConnection = @"Data Source={0};
                                                  Application Name=PUSWindows;
                                                  Initial Catalog=VersionDB;
                                                  Persist Security Info=True;
                                                  User ID=psutan;
                                                  Password=^1aS9zW>7+";
        internal static int _intProjectId;
        internal static bool _shouldStop;

        #endregion

        #region Get executive version

        internal static unsafe string GetVersionForAnyExecutive(string strFullFileName)
        {
            var fileInfo = new FileInfo(strFullFileName);

            int handle;
            int size = GetFileVersionInfoSize(fileInfo.FullName, out handle);

            var buffer = new byte[size];
            GetFileVersionInfo(fileInfo.FullName, 0, size, buffer);

            short* subBlock;
            //string subBlock;
            uint len;
            // Get the locale info from the version info:
            if (!VerQueryValue(buffer, @"\VarFileInfo\Translation", out subBlock, out len))
            {
                return string.Empty;
            }

            string spv = string.Format("{0}{1}{2}{3}", @"\StringFileInfo\", subBlock[0].ToString("X4"), subBlock[1].ToString("X4"), @"\ProductVersion");

            // Get the ProductVersion value for this program:
            IntPtr versionInfoPtr;
            try
            {
                if (!VerQueryValue(buffer, spv, out versionInfoPtr, out len))
                {
                    return string.Empty;
                }
            }
            catch (Win32Exception)
            {
                return string.Empty;
            }
            return Marshal.PtrToStringAnsi(versionInfoPtr);
        }

        [DllImport("version.dll")]
        public static extern bool GetFileVersionInfo(string sFileName,
            int handle, int size, byte[] infoBuffer);

        [DllImport("version.dll")]
        public static extern int GetFileVersionInfoSize(string sFileName,
            out int handle);

        [DllImport("version.dll")]
        public static extern bool VerQueryValue(byte[] pBlock,
            string pSubBlock, out IntPtr pValue, out uint len);

        [DllImport("version.dll")]
        public static extern unsafe bool VerQueryValue(byte[] pBlock,
            string pSubBlock, out short* pValue, out uint len);

        #endregion

        #region Processes methodes

        internal static void ShellNoWait(string strCommand, ProcessWindowStyle pStyle )
        {
            try
            {
                var startInfo = new ProcessStartInfo(strCommand);
                using (var myProcess = new Process())
                {
                    startInfo.UseShellExecute = true;
                    startInfo.WindowStyle = pStyle;
                    myProcess.StartInfo = startInfo;
                    myProcess.Start();
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }

        internal static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);

            //Loop through the running processes in with the same name  
            foreach (Process process in processes)
            {
                //Ignore the current process  
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file.  
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //Return the other process instance.  
                        return process;
                    }
                }
            }
            //No other instance was found, return null.  
            return null;
        }

        #endregion

        #region Test and Load New version

        internal static bool LoadNewVersions(string strHostName, string strHostIp)
        {
            bool blnUpdated = false;
            string strConnection = string.Format(MyClasses._strConnection, MyClasses._strServerName);

            try
            {
                var versionDb = new VersionDBDataContext(strConnection);
                string strAssemblyNames = string.Empty;
                foreach (var oneAssembly in (versionDb.AssemblyFiles.Where(oneAssembly => (oneAssembly.AssemblyProjectID == MyClasses._intProjectId))).ToList())
                {

                    if ((oneAssembly.AssemblyName.Equals(Settings.Default.ExecutableName)) && MyClasses.VersionCompare(MyClasses.GetVersionForAnyExecutive(oneAssembly.AssemblyName), oneAssembly.AssemblyVersion))
                    {
                        MyClasses.LoadAssemblyFromStore(oneAssembly.AssemblyName, MyClasses._intProjectId);
                        strAssemblyNames += string.Format("{0}, ", oneAssembly.AssemblyName);
                        blnUpdated = true;
                    }
                }

                if (blnUpdated)
                {
                    versionDb.UpdateHostLog02(MyClasses._intProjectId, strHostName, strHostIp, strAssemblyNames);
                }
                versionDb.Dispose();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            return blnUpdated;
        }

        internal static bool VersionCompare(string OldAssembly, string NewAssembly)
        {
            if (OldAssembly.Equals(string.Empty) || NewAssembly.Equals(string.Empty))
            {
                return false;
            }
            string[] OldVersion = OldAssembly.Split('.');
            string[] NewOldVersion = NewAssembly.Split('.');

            if (OldVersion.Length > 0 && NewOldVersion.Length > 0 && int.Parse(OldVersion[0]) < int.Parse(NewOldVersion[0]))
            {
                return true;
            }
            else if (OldVersion.Length > 1 && NewOldVersion.Length > 1 && int.Parse(OldVersion[1]) < int.Parse(NewOldVersion[1]))
            {
                return true;
            }
            else if (OldVersion.Length > 2 && NewOldVersion.Length > 2 && int.Parse(OldVersion[2]) < int.Parse(NewOldVersion[2]))
            {
                return true;
            }
            else if (OldVersion.Length > 3 && NewOldVersion.Length > 3 && int.Parse(OldVersion[3]) < int.Parse(NewOldVersion[3]))
            {
                return true;
            }
            return false;
        }

        internal static void LoadAssemblyFromStore(string strAssemblyName, int intAssemblyProjectID)
        {
            //_strExecutableName
            string strTempFileName = "Temp0219";
            _strExecutableNewName = Settings.Default.ExecutableNameNew;
            try
            {
                string strConnection = string.Format(MyClasses._strConnection, MyClasses._strServerName);
                if (File.Exists(_strExecutableNewName))
                {
                    File.Delete(_strExecutableNewName);
                }
                if (File.Exists(strTempFileName))
                {
                    File.Delete(strTempFileName);
                }

                byte[] readByteAssembly;
                using (var versionDb = new VersionDBDataContext(strConnection))
                {
                    readByteAssembly = versionDb.AssemblyFiles.Single(
                        one => one.AssemblyName == strAssemblyName && one.AssemblyProjectID == intAssemblyProjectID).AssemblyFiles.ToArray();
                }

                File.WriteAllBytes((strTempFileName), readByteAssembly);
                var readBlob = new FileStream(strTempFileName, FileMode.Open, FileAccess.Read);
                var destStream = new FileStream(_strExecutableNewName, FileMode.Create, FileAccess.Write);

                var compStream = new CompressedStream(readBlob);
                MyClasses.StreamCopy(compStream, destStream);
                destStream.Close();
                if (File.Exists(strTempFileName))
                {
                    File.Delete(strTempFileName);
                }

            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }

        internal static void StreamCopy(Stream sourceStream, Stream destStream)
        {
            try
            {
                int bytesRead;
                var buffer = new byte[32768];

                while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    destStream.Write(buffer, 0, bytesRead);
                }
            }
            finally
            {
                sourceStream.Close();
                destStream.Close();
            }
        }

        #endregion

        internal static void LoadBatchHandler()
        {
            string strConnection = string.Format(MyClasses._strConnection, MyClasses._strServerName);
            string strUpdaterName = Settings.Default.BatchHandlerName;
            string strUpdaterNameTemp = "Temp0382";
            try
            {
                var versionDb = new VersionDBDataContext(strConnection);
                string strAssemblyNames = string.Empty;
                if (File.Exists(strUpdaterName))
                {
                    File.Delete(strUpdaterName);
                }
                if (File.Exists(strUpdaterNameTemp))
                {
                    File.Delete(strUpdaterNameTemp);
                }
                foreach (var oneAssembly in (versionDb.AssemblyFiles.Where(oneAssembly => (oneAssembly.AssemblyProjectID == MyClasses._intProjectId))).ToList())
                {

                    if (oneAssembly.AssemblyName.Equals(strUpdaterName))
                    {
                        byte[] readByteAssembly;
                        readByteAssembly = versionDb.AssemblyFiles.Single(
                            one => one.AssemblyName == strUpdaterName && one.AssemblyProjectID == MyClasses._intProjectId).AssemblyFiles.ToArray();

                        File.WriteAllBytes((strUpdaterNameTemp), readByteAssembly);
                        var readBlob = new FileStream(strUpdaterNameTemp, FileMode.Open, FileAccess.Read);
                        var destStream = new FileStream(strUpdaterName, FileMode.Create, FileAccess.Write);

                        var compStream = new CompressedStream(readBlob);
                        MyClasses.StreamCopy(compStream, destStream);
                        destStream.Close();
                    }
                }

                if (File.Exists(strUpdaterNameTemp))
                {
                    File.Delete(strUpdaterNameTemp);
                }
                versionDb.Dispose();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            return;
        }

    }
}