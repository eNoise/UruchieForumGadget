using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace Uruchie.Core.Helpers
{
    /// <summary>
    /// Set of common extensions\helpers\utils
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// Try to execute an action
        /// </summary>
        public static void Try(this Action action)
        {
            try
            {
                if (action != null)
                    action();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Open browser with url as argument
        /// </summary>
        public static void OpenBrowser(string url)
        {
            //Process.Start(url); -- doesn't work with magnet's :-(

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
        
        /// <summary>
        /// Try parse string into Version
        /// </summary>
        public static Version TryParseVersion(string version)
        {
            if (string.IsNullOrEmpty(version))
                return null;

            try
            {
                return new Version(version);
            }
            catch {
                return null; }
        }
    }
}