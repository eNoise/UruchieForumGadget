using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Uruchie.Core;
using Uruchie.ForumGadjet.Helpers;
using Uruchie.ForumGadjet.Settings;
using Uruchie.ForumGadjet.Skins;

namespace Uruchie.ForumGadjet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Logger.LogFilePath =  Path.Combine(CommonUtils.GetInstallationFolder(), "UFG_Log.txt");
            Logger.Enabled = true;


            bool success = SkinManager.ChangeSkin(ConfigurationManager.CurrentConfiguration.Skin);

            App.Current.DispatcherUnhandledException += CurrentDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
            base.OnStartup(e);
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exc = e.ExceptionObject as Exception;
            Logger.LogError(exc, "UNHANDLED EXCEPTION!");
            if (e.ExceptionObject != null && !(e.ExceptionObject is Exception))
                Logger.LogDebug(e.ExceptionObject.ToString());
        }

        private static void CurrentDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Logger.LogError(e.Exception, "UNHANDLED EXCEPTION!");
        }
    }
}