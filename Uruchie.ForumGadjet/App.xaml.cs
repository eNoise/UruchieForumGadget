using System.Windows;
using System.Windows.Threading;
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
            bool success = SkinManager.ChangeSkin(ConfigurationManager.CurrentConfiguration.Skin);

            App.Current.DispatcherUnhandledException += CurrentDispatcherUnhandledException;
            base.OnStartup(e);
        }

        private static void CurrentDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Logger.LogError(e.Exception, "UNHANDLED EXCEPTION!");
        }
    }
}