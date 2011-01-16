using System.Runtime.Serialization;

namespace Uruchie.ForumGadjet.Model
{
    [DataContract]
    public class LogInfo
    {
        [DataMember(Name = "apimessages")]
        public LogMessage[] Messages { get; set; }

        [DataMember(Name = "error")]
        public string Error { get; set; }

        [DataMember(Name = "apiversion")]
        public string Version { get; set; }
    }
}