using System;
using System.IO;
using Uruchie.Core.Model;
using Uruchie.Core.Service;

namespace Uruchie.Core
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
            Log(string.Format(message, args), "DEBUG");
        }

        /// <summary>
        /// Log an a regular message
        /// </summary>
        public static void LogInfo(string message, params object[] args)
        {
            Log(string.Format(message, args), "INFO");
        }

        /// <summary>
        /// Log an a exception
        /// </summary>
        public static void LogError(Exception exc, string description, params object[] args)
        {
            if (exc != null)
                Log(string.Format("Exception: {0}; Description: {1}; Stacktrace: \n\n {2}", exc.Message, string.Format(description, args), exc.StackTrace), "ERROR");
        }

        /// <summary>
        /// Log short information about the current system
        /// </summary>
        public static void LogSystemInformation()
        {
            //TODO: .NET versions, OS version, Is64bit, RAM, CPU, GPU....
        }

        private static void Log(string message, string type)
        {
            if (!Enabled)
                return;

            File.AppendAllText(LogFilePath, type + ": " + message);
        }

        public static string LogFilePath { get; set; }

        public static bool Enabled { get; set; }
    }
}