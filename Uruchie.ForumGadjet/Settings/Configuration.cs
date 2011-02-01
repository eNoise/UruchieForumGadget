using System;
using System.Configuration;
using Uruchie.Core.Service;

namespace Uruchie.ForumGadjet.Settings
{
    public class Configuration
    {
        public int RefreshInterval { get; set; }
        public bool NewPostsAreAtTheTop { get; set; }
        public string Skin { get; set; }
        public ServiceSettings ServiceSettings { get; set; }
        public int PostLimit { get; set; }
        public string IgnoreNicks { get; set; }
        public string IgnorePosts { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}