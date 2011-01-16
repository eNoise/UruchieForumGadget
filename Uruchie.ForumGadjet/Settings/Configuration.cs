using System.Configuration;

namespace Uruchie.ForumGadjet.Settings
{
    public class Configuration : ConfigurationSection
    {
        [ConfigurationProperty("PostLimit", DefaultValue = 10)]
        [IntegerValidator(MaxValue = 100, MinValue = 10)]
        public int PostLimit { get; set; }

        [ConfigurationProperty("RefreshInterval", DefaultValue = 15)]
        [IntegerValidator(ExcludeRange = false, MaxValue = 1000, MinValue = 10)]
        public int RefreshInterval { get; set; }

        [ConfigurationProperty("RefreshInterval", DefaultValue = true)]
        public bool NewPostsAreAtTheTop { get; set; }

        [ConfigurationProperty("RefreshInterval", DefaultValue = "forum.uruchie.org")]
        public string ForumUrl { get; set; }

        [ConfigurationProperty("RefreshInterval", DefaultValue = "http://uruchie.org/api.php")]
        public string ApiUrl { get; set; }
    }
}