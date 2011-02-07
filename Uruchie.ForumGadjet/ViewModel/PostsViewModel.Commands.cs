using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using Uruchie.Core.Model;
using Uruchie.Core.Presentation;
using Uruchie.Core.Service;
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
        public RelayCommand PlusRating { get; set; }
        public RelayCommand MinusRating { get; set; }

        public void InitializeCommands()
        {
            LoadPosts = new RelayCommand(LoadPostsAsync);
            SelectNextPost = new RelayCommand(SelectNextPostAction, SelectNextPostCanExecute);
            SelectPreviousPost = new RelayCommand(SelectPreviousPostAction, SelectPreviousPostCanExecute);
            ReloadResources = new RelayCommand(ReloadResourcesAction);
            DisplaySettings = new RelayCommand(DisplaySettingsAction);

            PlusRating = new RelayCommand(PlusRatingAction);
            MinusRating = new RelayCommand(MinusRatingAction);
        }

        private void MinusRatingAction()
        {
            if (SelectedPost == null || string.IsNullOrEmpty(configuration.ServiceSettings.UserName) || 
                string.IsNullOrEmpty(configuration.ServiceSettings.PasswordHash))
                return;

            service.ChangeRating(SelectedPost.PostId, false, MinusRatingActionCallback);
        }

        private void MinusRatingActionCallback(OperationCompletedEventArgs<RatingOrKarmaChangeResult> obj)
        {
            if (obj.Error == null && obj.Result != null && (obj.Result.Error == null || !obj.Result.Error.HasError))
            {
                var post = Posts.FirstOrDefault(i => i.PostId == obj.Result.Id);
                if (post != null)
                    post.IncrementRating(false);
            }
        }

        private void PlusRatingAction()
        {
            if (SelectedPost == null || string.IsNullOrEmpty(configuration.ServiceSettings.UserName) ||
                string.IsNullOrEmpty(configuration.ServiceSettings.PasswordHash))
                return;

            service.ChangeRating(SelectedPost.PostId, true, PlusRatingActionCallback);
        }

        private void PlusRatingActionCallback(OperationCompletedEventArgs<RatingOrKarmaChangeResult> obj)
        {
            if (obj.Error == null && obj.Result != null && (obj.Result.Error == null || !obj.Result.Error.HasError))
            {
                var post = Posts.FirstOrDefault(i => i.PostId == obj.Result.Id);
                if (post != null)
                    post.IncrementRating(true);
            }
        }

        private void DisplaySettingsAction()
        {
            var dlg = new SettingsView();
            if (dlg.ShowDialog() == true)
            {
                ReloadConfiguration();
                LoadPostsAsync();
            }
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