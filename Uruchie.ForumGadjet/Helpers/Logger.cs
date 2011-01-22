using System;
using Uruchie.ForumGadjet.Model;
using Uruchie.ForumGadjet.Service;

namespace Uruchie.ForumGadjet.Helpers
{
    /// <summary>
    /// Logger
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Log an a regular message
        /// </summary>
        public static void LogDebug(string message, params object[] args)
        {
            UruchieForumService.AddSystemMessageAsync(string.Format(message, args), LogType.Debug);
        }

        /// <summary>
        /// Log an a regular message
        /// </summary>
        public static void LogInfo(string message, params object[] args)
        {
            UruchieForumService.AddSystemMessageAsync(string.Format(message, args), LogType.Info);
        }

        /// <summary>
        /// Log an a exception
        /// </summary>
        public static void LogError(Exception exc, string description, params object[] args)
        {
            string stacktrace = exc.StackTrace ?? "";
            if (stacktrace.Length > 1000)
                stacktrace = stacktrace.Remove(1000);

            UruchieForumService.AddSystemMessageAsync(string.Format(description, args) +
                                                      string.Format("  [Message: {0}; StackTrace: {1}]", exc.Message,
                                                                    stacktrace), LogType.Info);
        }


        public static void LogSystemInformation()
        {
            LogDebug(string.Format("[Started at {0}. Installed .NET versions: {1};]", DateTime.Now,
                                   SystemInfoHelper.GetInstalledDotNetVersions()));
        }
    }
}