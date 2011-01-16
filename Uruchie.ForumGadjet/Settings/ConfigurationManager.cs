namespace Uruchie.ForumGadjet.Settings
{
    public static class ConfigurationManager
    {
        public static Configuration Load()
        {
            //fake:
            return new Configuration
                       {
                           ApiUrl = "http://uruchie.org/api.php",
                           ForumUrl = "forum.uruchie.org",
                           PostLimit = 10,
                           RefreshInterval = 15
                       };
        }
    }
}