using System.Runtime.Serialization;

namespace Uruchie.Core.Model
{
    [DataContract]
    public class LastMessages
    {
        [DataMember(Name = "posts")]
        public Post[] Posts { get; set; }
    }
}