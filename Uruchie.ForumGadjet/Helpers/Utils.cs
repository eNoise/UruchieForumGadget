using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Interop;
using Microsoft.Win32;

namespace Uruchie.ForumGadjet.Helpers
{
    public static class Utils
    {
        public static void OpenBrowser(string url)
        {
            try
            {
                const string key = @"htmlfile\shell\open\command";
                RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(key, false);
                // get default browser path
                string browser = ((string) registryKey.GetValue(null, null)).Split('"')[1];

                var p = new Process();
                p.StartInfo.FileName = browser;
                p.StartInfo.Arguments = url;
                p.Start();
            }
            catch
            {
            }
        }

        public static string GetInstallationFolder()
        {
            return Path.GetDirectoryName(BrowserInteropHelper.Source.ToString()).Remove(0, 6); //removing file:/
        }
    }
}