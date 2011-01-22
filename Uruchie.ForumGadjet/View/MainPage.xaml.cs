using System.Windows.Controls;
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
            MouseLeave += (s, e) => ((PostsViewModel) DataContext).SelectedPost = null;
        }
    }
}