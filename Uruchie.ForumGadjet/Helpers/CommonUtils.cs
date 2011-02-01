using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Interop;
using Microsoft.Win32;

namespace Uruchie.ForumGadjet.Helpers
{
    public static class CommonUtils
    {
        public static string GetInstallationFolder()
        {
            return Path.GetDirectoryName(BrowserInteropHelper.Source.ToString()).Remove(0, 6); //removing file:/
        }
    }
}