using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Uruchie.Core.Model
{
    [DataContract]
    public class AuthenticationResult
    {
        [DataMember(Name = "authenticated")]
        public bool IsAuthenticated { get; set; }

        [DataMember(Name = "error")]
        public Error Error { get; set; }

        [DataMember(Name = "user")]
        public User User { get; set; }
    }
}
