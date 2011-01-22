﻿using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace Uruchie.ForumGadjet.Helpers
{
    public static class CommonHelper
    {
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

        public static void OpenBrowser(string url)
        {
            //Process.Start(url);

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
    }
}