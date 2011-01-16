using System;
using System.Reflection;
using System.Threading;

namespace Uruchie.ForumGadjet.Helpers
{
    /// <summary>
    /// Logger
    /// </summary>
    public static class Logger
    {
        private static string urlStatic = "";
        private static readonly string appVersion = "";

        static Logger()
        {
            appVersion = Assembly.GetAssembly(typeof (Logger)).GetName().Version.ToString(4);
        }

        public static void RegisterUrl(string url)
        {
            urlStatic = url;
        }

        /// <summary>
        /// Log an a regular message
        /// </summary>
        public static void LogMessage(string message, params object[] args)
        {
            if (string.IsNullOrEmpty(urlStatic)) return;

            string parameters = string.Format(
                    "module=forum&action=addlogmessage&app=ForumGadget&appversion={0}&logtype=DEBUG&logmessage={1}",
                    appVersion, string.Format(message, args));

            ThreadPool.QueueUserWorkItem(o => SendPost(parameters));
        }

        /// <summary>
        /// Log an a exception
        /// </summary>
        public static void LogError(Exception exc, string description, params object[] args)
        {
            if (string.IsNullOrEmpty(urlStatic)) return;

            string stacktrace = "";
            if (!string.IsNullOrEmpty(exc.StackTrace) && exc.StackTrace.Length > 1000)
                stacktrace = exc.StackTrace.Remove(1000);

            string parameters = string.Format(
                    "module=forum&action=addlogmessage&app=ForumGadget&appversion={0}&logtype=DEBUG&logmessage={1}",
                    appVersion, string.Format(" Description: {0}\r\n Message: {1};\r\n StackTrace: {2}",
                                              string.Format(description, args), exc.Message, stacktrace));


            ThreadPool.QueueUserWorkItem(o => SendPost(parameters));
        }

        private static void SendPost(string parameters)
        {
            try
            {
                HttpHelper.HttpPost(urlStatic, parameters);
            }
            catch { /*???*/ }
        }
    }
}