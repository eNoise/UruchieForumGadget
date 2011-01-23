using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
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
                string dir = CommonHelper.GetInstallationFolder();
                string skinDir = Path.Combine(dir, "Skins");
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

                string path = CommonHelper.GetInstallationFolder();
                string skinUrl = Path.Combine(Path.Combine(path, SkinsDirectory),
                                              Path.Combine(skinName, skinName + ".xaml"));

                var dictionary =
                    XamlReader.Load(File.Open(skinUrl, FileMode.Open, FileAccess.Read)) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dictionary);
                return true;
            }
            catch (Exception exc)
            {
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(
                    new ResourceDictionary
                        {
                            Source = new Uri(@"pack://application:,,,/Uruchie.ForumGadjet;component/Skins/DefaultSkin.xaml")
                        });

                return false;
            }
        }
    }
}