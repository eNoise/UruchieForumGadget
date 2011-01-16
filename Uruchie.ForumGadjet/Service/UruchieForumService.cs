using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using Uruchie.ForumGadjet.Helpers;

namespace Uruchie.ForumGadjet.Service
{
    public static class UruchieForumService
    {
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
                                                     callback(new OperationCompletedEventArgs<T>(null, false, o)
                                                                  {Result = obj});
                                                 }
                                                 catch (Exception exc)
                                                 {
                                                     Logger.LogError(exc, "Error during loading {0}. Url:{1}",
                                                                     typeof (T).Name, url + arguments);
                                                     callback(new OperationCompletedEventArgs<T>(exc, false, o));
                                                 }
                                             }, userState);
        }
    }
}