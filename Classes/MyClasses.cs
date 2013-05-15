using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

// Team foundation OK!

namespace ModulesLoader.Classes
{
    internal class MyClasses
    {

        #region Static Variables

        public static bool _blnNewLoaderIsLoaded;
        public static string _strNeedLoadUpdateFileName;
        public static string _strServerName;
        public static string _strExecutableName;
        public static string _strConnection = @"Data Source={0};Application Name=PUS_Windows;
                                                            Initial Catalog=VersionDB;
                                                            Persist Security Info=True;
                                                            User ID=psutan;
                                                            Password=^1aS9zW>7+";
        public static int _intProjectId;

        public static PUSWorker PUSUpdatesTestObject;
        public static Thread PUSUpdatesTestThread;

        #endregion

        public static void StreamCopy(Stream sourceStream, Stream destStream)
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

        #region Get executive version

        public static unsafe string GetVersionForAnyExecutive(string strFullFileName)
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

    }
}