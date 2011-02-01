using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Uruchie.Core.Model
{
    [DataContract]
    public class Error
    {
        [DataMember(Name="description")]
        public string Description { get; set; }

        [DataMember(Name = "type")]
        public string ErrorType { get; set; }

        [DataMember(Name = "error")]
        public bool HasError { get; set; }
    }
}
