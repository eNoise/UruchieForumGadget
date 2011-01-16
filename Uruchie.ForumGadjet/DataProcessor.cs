using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Uruchie.ForumGadjet.Helpers;
using Uruchie.ForumGadjet.Model;
using Uruchie.ForumGadjet.Settings;

namespace Uruchie.ForumGadjet
{
    public static class DataProcessor
    {
        public static IEnumerable<Post> PreparePosts(IEnumerable<Post> posts, Configuration configuration)
        {
            foreach (Post post in posts)
            {
                // 1. remove empty posts:
                if (string.IsNullOrEmpty(post.PageText))
                    continue;

                // 2. cut bb-codes:
                // 2.1. totally remove quotes
                //post.PageText = Regex.Replace(post.PageText, @"\[(.*?)\]", String.Empty, RegexOptions.IgnoreCase);
                post.PageText = Regex.Replace(post.PageText, @"\[(.*?)\]", String.Empty, RegexOptions.IgnoreCase);

                // 3. prepare calculated fields:
                post.PostUrl = string.Format("http://{0}/showthread.php?threadid={1}&p={2}&viewfull=1#post{2}",
                                             configuration.ForumUrl, post.Thread.ThreadId, post.PostId);

                // building url to avatar
                if (!string.IsNullOrEmpty(post.User.AvatarRevision) && post.User.AvatarRevision != "0")
                    post.User.AvatarUrl = string.Format("http://{0}/customavatars/avatar{1}_{2}.gif",
                                                        configuration.ForumUrl, post.User.UserId,
                                                        post.User.AvatarRevision);

                // 4. remove escaping
                post.Thread.Title = HttpHelper.Decode(post.Thread.Title);
                post.PageText = HttpHelper.Decode(post.PageText);
                post.User.UserName = HttpHelper.Decode(post.User.UserName);

                // 5. dattime (special case: convert from unix-time form into DateTime)
                try
                {
                    //deg returns time in unix-format and in GMT +0 (UTC)
                    post.DateTime =
                        (new DateTime(1970, 1, 1, 0, 0, 0)).AddSeconds(long.Parse(post.Dateline)).ToLocalTime();
                }
                catch
                {
                    post.DateTime = DateTime.Now;
                }

                yield return post;
            }
        }
    }
}