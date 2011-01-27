using System.Runtime.Serialization;

namespace Uruchie.Core.Model
{
    [DataContract]
    public class SystemMessage
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