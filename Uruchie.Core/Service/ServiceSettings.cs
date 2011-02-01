using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uruchie.Core.Service
{
    public class ServiceSettings
    {
        public string ApplicationName { get; set; }
        public string ApiUrl { get; set; }
        public string ForumUrl { get; set; }
        public string UpdatesFileUrl { get; set; }
        public Version Version { get; set; }

        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
