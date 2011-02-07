using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using Uruchie.Core;
using Uruchie.ForumGadjet.Helpers;

namespace Uruchie.ForumGadjet.Skins
{
    /// <summary>
    /// Skin manager
    /// </summary>
    public static class SkinManager
    {
        public const string DefaultSkin = "DefaultSkin";
        public const string SkinsDirectory = "Skins";

        /// <summary>
        /// Get installed skins
        /// </summary>
        public static string[] GetSkins()
        {
            try
            {
                string dir = CommonUtils.GetInstallationFolder();
                string skinDir = Path.Combine(dir, "Skins");

                if (!Directory.Exists(skinDir))
                    return new[] {DefaultSkin};

                List<string> skinDirectories = Directory.GetDirectories(skinDir).ToList();
                foreach (string skinDirectory in skinDirectories)
                {
                    if (Directory.GetFiles(skinDirectory,
                                           string.Format("{0}.xaml", Path.GetFileName(skinDirectory))).Length < 1)
                        skinDirectories.Remove(skinDirectory);
                }
                return skinDirectories.Select(Path.GetFileName).ToArray();
            }
            catch(Exception exc)
            {
                Logger.LogError(exc, "GetSkins() error");
                return new[] {DefaultSkin};
            }
        }

        /// <summary>
        /// Apply skin
        /// </summary>
        public static bool ChangeSkin(string skinName)
        {
            try
            {
                if (string.IsNullOrEmpty(skinName))
                    throw new ArgumentException();

                string path = CommonUtils.GetInstallationFolder();
                string skinUrl = Path.Combine(Path.Combine(path, SkinsDirectory),
                                              Path.Combine(skinName, skinName + ".xaml"));

                if (!File.Exists(skinUrl))
                {
                    ChangeSkinToDefault();
                    return false;
                }

                var dictionary =
                    XamlReader.Load(File.Open(skinUrl, FileMode.Open, FileAccess.Read)) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dictionary);
                return true;
            }
            catch
            {
                ChangeSkinToDefault();
                return false;
            }
        }

        private static void ChangeSkinToDefault()
        {
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(
                new ResourceDictionary
                {
                    // if we can't 
                    Source = new Uri(@"pack://application:,,,/Uruchie.ForumGadjet;component/Skins/DefaultSkin.xaml")
                });
        }
    }
}