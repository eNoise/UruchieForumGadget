using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Uruchie.Core.Model
{
    [DataContract]
    public class RatingOrKarmaChangeResult
    {
        [DataMember(Name="sended")]
        public bool Sended { get; set; }

        [DataMember(Name = "error")]
        public Error Error { get; set; }

        public string Id { get; set; }
    }
}
