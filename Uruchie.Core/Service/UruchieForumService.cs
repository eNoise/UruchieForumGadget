using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using Uruchie.Core.Helpers;
using Uruchie.Core.Model;
using Uruchie.Core.Presentation;

namespace Uruchie.Core.Service
{
    /// <summary>
    /// Uruchie forum service
    /// </summary>
    public class UruchieForumService : IUruchieForumService
    {
        private readonly string lastMessageFields;

        public UruchieForumService(ServiceSettings settings)
        {
            Settings = settings;

            if (ViewModelBase.IsInDesignModeStatic)
            {
                AppVersion = "DesignTime";
                lastMessageFields = "";
            }

            AppVersion = Assembly.GetAssembly(typeof (UruchieForumService)).GetName().Version.ToString(4);
            lastMessageFields = ReflectionHelper.GetActiveDataMembers(typeof (Post));
        }

        public ServiceSettings Settings { get; set; }

        /// <summary>
        /// Current application version
        /// </summary>
        public string AppVersion { get; private set; }

        /// <summary>
        /// Deserialize the json string into strong typed object
        /// </summary>
        private static T DeserializeJson<T>(string json) where T : class
        {
            if (string.IsNullOrEmpty(json))
                return null;

            byte[] byteArray = Encoding.ASCII.GetBytes(json);
            var stream = new MemoryStream(byteArray);

            try
            {
                var serializer = new DataContractJsonSerializer(typeof (T));
                return serializer.ReadObject(stream) as T;
            }
            catch (Exception exc)
            {
                Logger.LogError(exc, "Error during json deserialization for type " + typeof (T).Name);
                return null;
            }
            finally
            {
                stream.Close();
            }
        }

        /// <summary>
        /// Async data loading
        /// </summary>
        private static void LoadDataAsync<T>(string url, string arguments, Action<OperationCompletedEventArgs<T>> callback, object userState = null)
            where T : class
        {
            ThreadPool.QueueUserWorkItem(o =>
                {
                    try
                    {
                        var obj = DeserializeJson<T>(HttpHelper.HttpPost(url, arguments));
                        if (callback != null)
                            callback(new OperationCompletedEventArgs<T>(null, false, o)
                                        {Result = obj});
                    }
                    catch (Exception exc)
                    {
                        Logger.LogError(exc, "Error during loading {0}. Url:{1}",
                                        typeof (T).Name, url + arguments);
                        if (callback != null)
                            callback(new OperationCompletedEventArgs<T>(exc, false, o));
                    }
                }, userState);
        }

        /// <summary>
        /// Add system message (log)
        /// </summary>
        public void AddSystemMessageAsync(string message, SystemMessageType logtype)
        {
            if (ViewModelBase.IsInDesignModeStatic)
                return;

            string parameters = string.Format(
                "module=forum&action=addlogmessage&app={3}&appversion={0}&logtype={1}&logmessage={2}",
                AppVersion, logtype, message, Settings.ApplicationName);
            LoadDataAsync<SystemMessage>(Settings.ApiUrl, parameters, null);
        }

        /// <summary>
        /// Load collection of last added posts
        /// </summary>
        public void LoadLastPostsAsync(int postLimit, Action<OperationCompletedEventArgs<LastMessages>> callback)
        {
            string postsQuery = string.Format("module=forum&action=lastmessages&limit={0}{1}", postLimit,
                                              string.IsNullOrEmpty(lastMessageFields) ? string.Empty : "&filter=" + lastMessageFields);

            LoadDataAsync<LastMessages>(Settings.ApiUrl, postsQuery, e =>
                                                           {
                                                               if (e.Error == null && e.Result != null && e.Result.Posts != null)
                                                                   e.Result.Posts = PostCollectionProcessor.PreparePosts(Settings, e.Result.Posts).ToArray();
                                                               callback(e);
                                                           });
        }

        /// <summary>
        /// Returns actual version
        /// </summary>
        public void CheckIfNewerVersionExists(Action<Version> callback)
        {
            var client = new WebClient();
            client.DownloadStringCompleted +=
                (s, e) => callback(CommonHelper.TryParseVersion(e.Result));
            client.DownloadStringAsync(new Uri(Settings.UpdatesFileUrl));
        }

        /// <summary>
        /// Try to authenticate
        /// </summary>
        public void Authenticate(Action<OperationCompletedEventArgs<AuthenticationResult>> callback)
        {
            string query = string.Format("module=forum&action=login&username={0}&password={1}", Settings.UserName, Settings.PasswordHash);
            LoadDataAsync(Settings.ApiUrl, query, callback);
        }

        /// <summary>
        /// Change rating
        /// </summary>
        public void ChangeRating(string postId, bool positive, Action<OperationCompletedEventArgs<RatingOrKarmaChangeResult>> callback)
        {
            string query = string.Format("module=forum&action={2}&username={0}&password={1}&postid={3}",
                Settings.UserName, Settings.PasswordHash, positive ? "plusrating" : "minusrating", postId);
            LoadDataAsync<RatingOrKarmaChangeResult>(Settings.ApiUrl, query, e =>
                                                        {
                                                            if (e.Error == null || e.Result != null)
                                                               e.Result.Id = postId;
                                                            callback(e);
                                                        });
        }

        /// <summary>
        /// Change karma
        /// </summary>
        public void ChangeKarma(string userName, bool positive, Action<OperationCompletedEventArgs<RatingOrKarmaChangeResult>> callback)
        {
            string query = string.Format("module=forum&action={2}&username={0}&password={1}&username={3}",
                Settings.UserName, Settings.PasswordHash, positive ? "pluskarma" : "minuskarma", userName);
            LoadDataAsync<RatingOrKarmaChangeResult>(Settings.ApiUrl, query, e =>
                                                        {
                                                            if (e.Error == null || e.Result != null)
                                                                e.Result.Id = userName;
                                                            callback(e);
                                                        });
        }
    }
}