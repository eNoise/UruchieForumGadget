using System.Runtime.Serialization;

namespace Uruchie.ForumGadjet.Model
{
    [DataContract]
    public class Thread
    {
        [DataMember(Name = "threadid")]
        public string ThreadId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}