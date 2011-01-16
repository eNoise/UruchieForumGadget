using System.Runtime.Serialization;
using Uruchie.ForumGadjet.Helpers.Mvvm;

namespace Uruchie.ForumGadjet.Model
{
    [DataContract]
    public class LastMessages : PropertyChangedBase
    {
        [DataMember(Name = "posts")]
        public Post[] Posts { get; set; }
    }
}