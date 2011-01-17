using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Uruchie.ForumGadjet.ViewModel;

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
            //deselect post when mouse leaves view
            //TODO: replace with EventToCommand 
            MouseLeave += (s,e) => ((PostsViewModel)DataContext).SelectedPost = null; 
        }
    }
}