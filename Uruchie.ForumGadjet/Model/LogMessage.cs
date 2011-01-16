using System.Runtime.Serialization;

namespace Uruchie.ForumGadjet.Model
{
    [DataContract]
    public class LogMessage
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }
}