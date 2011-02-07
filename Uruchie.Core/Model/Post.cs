using System;
using System.Runtime.Serialization;
using Uruchie.Core.Presentation;

namespace Uruchie.Core.Model
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

        private string rating;

        [DataMember(Name = "post_rating")]
        public string Rating
        {
            get { return rating; }
            set
            {
                rating = value;
                RaisePropertyChanged("Rating");
            }
        }

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

        public void IncrementRating(bool positive)
        {
            int ratingNum = 0;
            if (!string.IsNullOrEmpty(Rating) && int.TryParse(Rating, out ratingNum))
                Rating = (positive ? ratingNum + 1 : ratingNum - 1).ToString();
        }
    }
}