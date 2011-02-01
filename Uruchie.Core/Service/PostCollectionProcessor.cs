using System;
using System.Collections.Generic;
using Uruchie.Core.Helpers;
using Uruchie.Core.Model;

namespace Uruchie.Core.Service
{
    public static class PostCollectionProcessor
    {
        public static IEnumerable<Post> PreparePosts(ServiceSettings settings, IEnumerable<Post> posts)
        {
            foreach (Post post in posts)
            {
                // 1. remove empty posts:
                if (string.IsNullOrEmpty(post.PageText))
                    continue;

                // 3. prepare calculated fields:
                post.PostUrl = string.Format("http://{0}/showthread.php?threadid={1}&p={2}&viewfull=1#post{2}",
                                             settings.ForumUrl, post.Thread.ThreadId, post.PostId);

                // building url to avatar
                if (!string.IsNullOrEmpty(post.User.AvatarRevision) && post.User.AvatarRevision != "0")
                    post.User.AvatarUrl = string.Format("http://{0}/customavatars/avatar{1}_{2}.gif",
                                                        settings.ForumUrl, post.User.UserId,
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