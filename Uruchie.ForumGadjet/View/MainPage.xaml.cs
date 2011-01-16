using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Uruchie.ForumGadjet.View
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            MouseLeave += MainPageMouseLeave;
        }

        private void MainPageMouseLeave(object sender, MouseEventArgs e)
        {
            listbox.SelectedItem = null;
        }

        private void HyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            e.Handled = true;
            if (e.Uri != null)
                Process.Start(e.Uri.ToString());
        }

        private void rootBorder_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void rootBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}