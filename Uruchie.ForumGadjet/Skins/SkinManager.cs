using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

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
                string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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
            catch
            {
                return new[] {DefaultSkin};
            }
        }

        /// <summary>
        /// Apply skin
        /// </summary>
        public static void ChangeSkin(string skinName)
        {
            try
            {
                string path = Path.GetDirectoryName(Assembly.GetAssembly(typeof (SkinManager)).Location);
                string skinUrl = Path.Combine(Path.Combine(path, SkinsDirectory),
                                              Path.Combine(skinName, skinName + ".xaml"));

                var dictionary =
                    XamlReader.Load(File.Open(skinUrl, FileMode.Open, FileAccess.Read)) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dictionary);
            }
            catch
            {
            }
        }
    }
}