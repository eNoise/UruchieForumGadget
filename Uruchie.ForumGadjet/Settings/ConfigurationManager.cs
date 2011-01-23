using System;
using System.IO;
using System.Xml.Serialization;
using Uruchie.ForumGadjet.Helpers;
using Uruchie.ForumGadjet.Helpers.Mvvm;
using Uruchie.ForumGadjet.Skins;

namespace Uruchie.ForumGadjet.Settings
{
    public static class ConfigurationManager
    {
        private static Configuration currentConfiguration;
        private static readonly XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
        public const string ConfigFileName = "Configuration.xml";
        
        public static Configuration CurrentConfiguration
        {
            get
            {
                if (currentConfiguration == null)
                    Reload();
                return currentConfiguration;
            }
            private set { currentConfiguration = value; }
        }

        public static void Reload()
        {
            var cfg = LoadOrDefault();
            if (IsValid(cfg))
                CurrentConfiguration = cfg;
            else
                CurrentConfiguration = CreateDefault();
        }

        private static bool IsValid(Configuration config)
        {
            if (config == null)
                return false;
            if (config.PostLimit < 1 || config.PostLimit > 100)
                return false;
            if (config.RefreshInterval < 10 || config.RefreshInterval > 1000)
                return false;
            return true;
        }

        public static void Save()
        {
            try
            {
                string file = GetConfigPath();
                if (File.Exists(file))
                    File.Delete(file);
                using (var stream = File.Create(GetConfigPath()))
                    xmlSerializer.Serialize(stream, CurrentConfiguration);
            }
            catch(Exception exc)
            {
                Logger.LogError(exc, "Save() error");
            }
        }

        private static Configuration LoadOrDefault()
        {
            if (ViewModelBase.IsInDesignModeStatic || !File.Exists(GetConfigPath()))
                return CreateDefault();

            try
            {
                using (var stream = File.Open(GetConfigPath(), FileMode.Open, FileAccess.Read))
                    return xmlSerializer.Deserialize(stream) as Configuration;
            } 
            catch(Exception exc)
            {
                return CreateDefault();
            }
        }

        private static string GetConfigPath()
        {
            return Path.Combine(CommonHelper.GetInstallationFolder(), ConfigFileName);
        }

        private static Configuration CreateDefault()
        {
            return new Configuration
                       {
                           ApiUrl = "http://uruchie.org/api.php",
                           ForumUrl = "forum.uruchie.org",
                           PostLimit = 10,
                           RefreshInterval = 60,
                           Skin = SkinManager.DefaultSkin
                       };
        }
    }
}