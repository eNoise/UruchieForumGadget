using System;
using System.Runtime.Serialization;

namespace Uruchie.Core.Model
{
    [DataContract]
    public class Post
    {
        [DataMember(Name = "postid")]
        public string PostId { get; set; }

        [DataMember(Name = "pagetext")]
        public string PageText { get; set; }

        [DataMember(Name = "thread")]
        public Thread Thread { get; set; }

        [DataMember(Name = "post_rating")]
        public string Rating { get; set; }

        [DataMember(Name = "user")]
        public User User { get; set; }

        [DataMember(Name = "dateline")]
        public string Dateline { get; set; }

        public string PostUrl { get; set; }

        public DateTime DateTime { get; set; }

        public bool PostIsBullshit
        {
            get
            {
                return !string.IsNullOrEmpty(Rating) && Rating.Contains("-");
            }
        }

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