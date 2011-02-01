using System;
using System.Net;
using Uruchie.Core.Model;
using Uruchie.Core.Service;

namespace Uruchie.Core
{
    public interface IUruchieForumService
    {
        /// <summary>
        /// Add system message (log)
        /// </summary>
        void AddSystemMessageAsync(string message, SystemMessageType logtype);

        /// <summary>
        /// Load collection of last added posts
        /// </summary>
        void LoadLastPostsAsync(int postLimit, Action<OperationCompletedEventArgs<LastMessages>> callback);

        /// <summary>
        /// Returns actual version
        /// </summary>
        void CheckIfNewerVersionExists(Action<Version> callback);

        /// <summary>
        /// Try to authenticate
        /// </summary>
        void Authenticate(Action<OperationCompletedEventArgs<AuthenticationResult>> callback);

        /// <summary>
        /// Change rating
        /// </summary>
        void ChangeRating(string postId, bool positive, Action<OperationCompletedEventArgs<RatingOrKarmaChangeResult>> callback);

        /// <summary>
        /// Change karma
        /// </summary>
        void ChangeKarma(string userName, bool positive, Action<OperationCompletedEventArgs<RatingOrKarmaChangeResult>> callback);
    }
}