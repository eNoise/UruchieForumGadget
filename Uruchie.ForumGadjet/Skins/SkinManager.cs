using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Uruchie.ForumGadjet.Skins
{
    public static class SkinManager
    {
        public const string DefaultSkin = "DefaultSkin";

        public static string[] GetSkins()
        {
            try
            {
                string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                var skinDirectories = Directory.GetDirectories(dir).ToList();
                foreach (var skinDirectory in skinDirectories)
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
    }
}
