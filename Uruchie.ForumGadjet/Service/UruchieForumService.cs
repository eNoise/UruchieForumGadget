using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using Uruchie.ForumGadjet.Helpers;
using Uruchie.ForumGadjet.Helpers.Mvvm;
using Uruchie.ForumGadjet.Model;
using Uruchie.ForumGadjet.Settings;
using Thread = Uruchie.ForumGadjet.Model.Thread;

namespace Uruchie.ForumGadjet.Service
{
    public class UruchieForumService
    {
        private static readonly string appVersion;
        private static readonly string lastMessageFields;

        static UruchieForumService()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                appVersion = "DesignTime";
                lastMessageFields = "";
            }

            appVersion = Assembly.GetAssembly(typeof (Logger)).GetName().Version.ToString(4);
            lastMessageFields = ReflectionHelper.GetActiveDataMembers(typeof (Post), typeof (User), typeof (Thread));
        }

        /// <summary>
        /// Deserialize the json string into strong typed object
        /// </summary>
        private static T Deserialize<T>(string json) where T : class
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
        public static void LoadDataAsync<T>(string url, string arguments,
                                            Action<OperationCompletedEventArgs<T>> callback, object userState = null)
            where T : class
        {
            ThreadPool.QueueUserWorkItem(o =>
                                             {
                                                 try
                                                 {
                                                     var obj = Deserialize<T>(HttpHelper.HttpPost(url, arguments));
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
                                             },
                                         userState);
        }

        /// <summary>
        /// </summary>
        public static void AddSystemMessageAsync(string message, LogType logtype)
        {
            if (ViewModelBase.IsInDesignModeStatic)
                return;

            string parameters = string.Format(
                "module=forum&action=addlogmessage&app=ForumGadget&appversion={0}&logtype={1}&logmessage={2}",
                appVersion, logtype, message);
            LoadDataAsync<LogInfo>(ConfigurationManager.CurrentConfiguration.ApiUrl, parameters, null);
        }

        /// <summary>
        /// </summary>
        public static void LoadLastPostsAsync(Action<OperationCompletedEventArgs<LastMessages>> callback)
        {
            string postsQuery = string.Format("module=forum&action=lastmessages&limit={0}{1}",
                                              ConfigurationManager.CurrentConfiguration.PostLimit,
                                              string.IsNullOrEmpty(lastMessageFields) ? string.Empty : "&filter=" + lastMessageFields);

            LoadDataAsync(ConfigurationManager.CurrentConfiguration.ApiUrl, postsQuery, callback);
        }
    }
}