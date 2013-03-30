using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using ModulesLoader.Classes;
using ModulesLoader.Data;
using ModulesLoader.Properties;
using Xceed.Compression;

namespace ModulesLoader
{
    internal class Program
    {
        
        #region Variables
        
        private static string _strNeedLoadUpdateFileName;
        private static string _strServerName;
        private static string _strExecutableName;
        
        private static int _intProjectId;
        
        private static FrmProgressClass _frmProgress;
        private static string _strLoaderName;
        private static string _strLoaderVersion;
        
        private static string _strHostIp;
        private static string _strHostName;
        private static string _strConnection;
        
        #endregion
        
        [STAThread]
        private static void Main()
        {
            try
            {
                Licenser.LicenseKey = "ZIN37-U8JE5-P55Z8-YKCA";
                var objEntry = Dns.GetHostEntry(Dns.GetHostName());
                _strHostName = objEntry.HostName;
                try
                {
                    _strHostIp = objEntry.AddressList.Where(oneIP => oneIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToArray()[0].ToString();
                }
                catch (Exception)
                {
                    _strHostIp = "192.168.";
                } 
                var insAssembly = Assembly.GetExecutingAssembly();
                _strLoaderVersion = insAssembly.GetName().Version.ToString();
                _strLoaderName = insAssembly.GetName().Name;
                
                _intProjectId = Settings.Default.ProjectID;
                
                _strExecutableName = Settings.Default.ExecutableName;
                _strNeedLoadUpdateFileName = Settings.Default.NeedLoadUpdateFileName;

                if (_strHostIp.StartsWith("192.168."))
                {
                    _strServerName = Settings.Default.ServerName;
                }
                else
                {
                    _strServerName = Settings.Default.ServerIP;
                }
                _strServerName = Settings.Default.ServerName;

                _strConnection = string.Format(@"Data Source={0};Application Name=PUS_Windows;
                                                 Initial Catalog=VersionDB;
                                                 Persist Security Info=True;
                                                 User ID=psutan;
                                                 Password=^1aS9zW>7+",
                    _strServerName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}{1}", Resources.Program_Main_ERROR, ex.Message),
                    string.Format("{0}{1}", Resources.Program_Main_Loader_version, _strLoaderVersion), MessageBoxButtons.OK);
                return;
            }
            CheckNewVersions();
        }
        
        private static void CheckNewVersions()
        {
            try
            {
                var versionDb = new VersionDBDataContext(_strConnection);
                int intUpdateNumber = 0;    
                int intNewNeedUpdate = versionDb.tUpdateNumbers.Single(
                    one => one.AssemblyProjectID == _intProjectId).UpdateNumber;
                    
                if (!File.Exists(_strNeedLoadUpdateFileName) ||
                    int.TryParse(File.ReadAllText(_strNeedLoadUpdateFileName), out intUpdateNumber) &&
                    intUpdateNumber < intNewNeedUpdate)
                {
                    LoadNewVersions();
                    File.WriteAllText(_strNeedLoadUpdateFileName, intNewNeedUpdate.ToString());
                }
                if (File.Exists(_strExecutableName))
                {
                    ShellNoWait(_strExecutableName);
                }
                versionDb.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}{1}", Resources.Program_Main_ERROR, ex.Message),
                    string.Format("{0}{1}", Resources.Program_Main_Loader_version, _strLoaderVersion), MessageBoxButtons.OK);
            }
        }
        
        private static void LoadNewVersions()
        {
            bool blnUpdated = false;
            try
            {
                var versionDb = new VersionDBDataContext(_strConnection);
                _frmProgress = new FrmProgressClass
                {
                    Text = string.Format("{0}{1}", Resources.Program_Main_Loader_version, _strLoaderVersion),
                    pbrProgressBar = { Value = 0, Minimum = 0, Maximum = versionDb.AssemblyFiles.Count() }
                };
                
                _frmProgress.Show();
                string strAssemblyNames = string.Empty;
                foreach (var oneAssembly in 
                    (versionDb.AssemblyFiles.Where(oneAssembly => (oneAssembly.AssemblyProjectID == _intProjectId))).ToList())
                {
                    Application.DoEvents();
                    _frmProgress.lblAssemblyName.Text = oneAssembly.AssemblyName;

                    bool blnIsLoader = _strLoaderName.Equals(Path.GetFileNameWithoutExtension(oneAssembly.AssemblyName)); 
                        
                    if ((blnIsLoader && VersionCompare(_strLoaderVersion, oneAssembly.AssemblyVersion)) ||
                        (!blnIsLoader && (!File.Exists(oneAssembly.AssemblyName) ||
                                          VersionCompare(MyClasses.GetVersionForAnyExecutive(oneAssembly.AssemblyName),
                                              oneAssembly.AssemblyVersion))))
                    {
                        LoadAssemblyFromStore(oneAssembly.AssemblyName, _intProjectId, oneAssembly.Compressed);
                        strAssemblyNames += string.Format("{0}, ", oneAssembly.AssemblyName);
                        blnUpdated = true;
                        _frmProgress.pbrProgressBar.Value += 1;
                        _frmProgress.Refresh();
                    }
                }
                _frmProgress.Close();
                _frmProgress.Dispose();
                
                if (blnUpdated)
                {
                    versionDb.UpdateHostLog02(_intProjectId, _strHostName, _strHostIp, strAssemblyNames);
                }
                versionDb.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}{1}", Resources.Program_Main_ERROR, ex.Message),
                    string.Format("{0}{1}", Resources.Program_Main_Loader_version, _strLoaderVersion), MessageBoxButtons.OK);
            }
        }
        
        private static bool VersionCompare(string OldAssembly, string NewAssembly)
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
        
        private static void LoadAssemblyFromStore(string strAssemblyName, int intAssemblyProjectID, bool blnCompressed)
        {
            try
            {
                byte[] readByteAssembly;
                using (var versionDb = new VersionDBDataContext(_strConnection))
                {
                    readByteAssembly = versionDb.AssemblyFiles.Single(
                        one => one.AssemblyName == strAssemblyName && one.AssemblyProjectID == intAssemblyProjectID).AssemblyFiles.ToArray();
                }
                
                string strFileName = string.Format("{0}{1}", strAssemblyName, (blnCompressed) ? ".zip" : "");
                
                if (File.Exists(strFileName))
                {
                    File.Delete(strFileName);
                }
                File.WriteAllBytes((strFileName), readByteAssembly);
                
                if (blnCompressed)
                {
                    var destStream = new FileStream(strAssemblyName, FileMode.Create, FileAccess.Write);
                    var readBlob = new FileStream(string.Format("{0}.zip", strAssemblyName), FileMode.Open, FileAccess.Read);
                    
                    var compStream = new CompressedStream(readBlob);
                    MyClasses.StreamCopy(compStream, destStream);
                    destStream.Close();
                    if (File.Exists(string.Format("{0}.zip", strAssemblyName)))
                    {
                        File.Delete(string.Format("{0}.zip", strAssemblyName));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}{1}", Resources.Program_Main_ERROR, ex.Message),
                    string.Format("{0}{1}", Resources.Program_Main_Loader_version, _strLoaderVersion), MessageBoxButtons.OK);
            }
        }
        
        public static void ShellNoWait(string strCommand)
        {
            try
            {
                var startInfo = new ProcessStartInfo(strCommand);
                using (var myProcess = new Process())
                {
                    startInfo.UseShellExecute = true;
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                    myProcess.StartInfo = startInfo;
                    myProcess.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}{1}", Resources.Program_Main_ERROR, ex.Message),
                    string.Format("{0}{1}", Resources.Program_Main_Loader_version, _strLoaderVersion), MessageBoxButtons.OK);
            }
        }
    }
}