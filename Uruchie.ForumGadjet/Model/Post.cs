using System;
using System.Runtime.Serialization;
using Uruchie.ForumGadjet.Helpers.Mvvm;

namespace Uruchie.ForumGadjet.Model
{
    [DataContract]
    public class Post : PropertyChangedBase
    {
        [DataMember(Name = "postid")]
        public string PostId { get; set; }

        [DataMember(Name = "pagetext")]
        public string PageText { get; set; }

        [DataMember(Name = "thread")]
        public Thread Thread { get; set; }

        [DataMember(Name = "user")]
        public User User { get; set; }

        [DataMember(Name = "dateline")]
        public string Dateline { get; set; }

        public string PostUrl { get; set; }

        public DateTime DateTime { get; set; }

        public override string ToString()
        {
            return string.Format("id: {0}, text: {1}", PostId, PageText);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Post))
                return false;
            return PostId.Equals(((Post) obj).PostId);
        }

        public override int GetHashCode()
        {
            return PostId.GetHashCode();
        }
    }
}