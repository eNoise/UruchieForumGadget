using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using Uruchie.ForumGadjet.Helpers.Mvvm;
using Uruchie.ForumGadjet.View;

namespace Uruchie.ForumGadjet.ViewModel
{
    partial class PostsViewModel
    {
        public RelayCommand SelectNextPost { get; set; }
        public RelayCommand SelectPreviousPost { get; set; }
        public RelayCommand LoadPosts { get; set; }
        public RelayCommand ReloadResources { get; set; }
        public RelayCommand DisplaySettings { get; set; }

        public void InitializeCommands()
        {
            LoadPosts = new RelayCommand(LoadPostsAsync);
            SelectNextPost = new RelayCommand(SelectNextPostAction, SelectNextPostCanExecute);
            SelectPreviousPost = new RelayCommand(SelectPreviousPostAction, SelectPreviousPostCanExecute);
            ReloadResources = new RelayCommand(ReloadResourcesAction);
            DisplaySettings = new RelayCommand(DisplaySettingsAction);
        }

        private void DisplaySettingsAction()
        {
            var dlg = new SettingsView();
            dlg.ShowDialog();
        }

        private void ReloadResourcesAction()
        {
            string path = Path.GetDirectoryName(Assembly.GetAssembly(GetType()).Location);
            string skinUrl = Path.Combine(path, Path.Combine(CurrentSkin, CurrentSkin + ".xaml"));

            var dictionary = XamlReader.Load(File.Open(skinUrl, FileMode.Open, FileAccess.Read)) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }

        private bool SelectNextPostCanExecute()
        {
            return Posts != null &&
                   Posts.Any() &&
                   SelectedPost != null &&
                   Posts.IndexOf(SelectedPost) < Posts.Count - 1;
        }

        private void SelectNextPostAction()
        {
            try
            {
                SelectedPost = Posts[Posts.IndexOf(SelectedPost) + 1];
            }
            catch
            {
                //todo: log? 
            }
        }

        private bool SelectPreviousPostCanExecute()
        {
            bool canExecuted = Posts != null &&
                               Posts.Any() &&
                               SelectedPost != null &&
                               Posts[0] != SelectedPost;

            return canExecuted;
        }

        private void SelectPreviousPostAction()
        {
            try
            {
                int index = Posts.IndexOf(SelectedPost);
                SelectedPost = Posts[index - 1];
            }
            catch
            {
                //todo: log? 
            }
        }
    }
}