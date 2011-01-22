using Uruchie.ForumGadjet.Skins;

namespace Uruchie.ForumGadjet.Settings
{
    public static class ConfigurationManager
    {
        private static Configuration currentConfiguration;

        static ConfigurationManager()
        {
            CurrentConfiguration = new Configuration();
        }

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
            CurrentConfiguration = CreateDefault();
        }

        public static void Save()
        {
        }

        private static Configuration CreateDefault()
        {
            return new Configuration
                       {
                           ApiUrl = "http://uruchie.org/api.php",
                           ForumUrl = "forum.uruchie.org",
                           PostLimit = 20,
                           RefreshInterval = 30,
                           Skin = SkinManager.DefaultSkin
                       };
        }
    }
}