using System.Runtime.Serialization;

namespace Uruchie.ForumGadjet.Model
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "userid")]
        public string UserId { get; set; }

        [DataMember(Name = "username")]
        public string UserName { get; set; }

        [DataMember(Name = "avatarrevision")]
        public string AvatarRevision { get; set; }

        public string AvatarUrl { get; set; }
    }
}