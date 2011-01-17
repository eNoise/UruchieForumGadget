using System.Runtime.Serialization;

namespace Uruchie.ForumGadjet.Model
{
    [DataContract]
    public class LogMessage
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "ipaddress")]
        public string IpAddress { get; set; }

        [DataMember(Name = "dateline")]
        public string Dateline { get; set; }

        [DataMember(Name = "appversion")]
        public string Version { get; set; }

        [DataMember(Name = "messagetype")]
        public string MessageType { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }
}