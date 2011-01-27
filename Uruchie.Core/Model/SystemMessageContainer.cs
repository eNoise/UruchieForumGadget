using System.Runtime.Serialization;

namespace Uruchie.Core.Model
{
    [DataContract]
    public class SystemMessageContainer
    {
        [DataMember(Name = "apimessages")]
        public SystemMessage[] Messages { get; set; }

        [DataMember(Name = "error")]
        public string Error { get; set; }

        [DataMember(Name = "apiversion")]
        public string Version { get; set; }
    }
}