using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using Uruchie.Core;
using Uruchie.Core.Helpers;
using Uruchie.Core.Model;
using Uruchie.Core.Presentation;
using Uruchie.Core.Service;
using Uruchie.ForumGadjet.Settings;

namespace Uruchie.ForumGadjet.ViewModel
{
    /// <summary>
    /// ViewModel for Posts View
    /// </summary>
    public partial class PostsViewModel : ViewModelBase
    {
        private IUruchieForumService service;
        private readonly DispatcherTimer timer;
        private Configuration configuration;
        private string currentSkin;
        private bool isError;
        private string errorMessage;
        private bool isFirstLoading;
        private ObservableCollection<Post> posts;
        private Post selectedPost;


        public PostsViewModel()
        {
            ReloadConfiguration();
            service = new UruchieForumService(configuration.ServiceSettings);
            InitializeCommands();

            if (!IsInDesignMode)
                IsFirstLoading = true;

            LoadPostsAsync();

            if (IsInDesignMode)
                return;

            Logger.LogSystemInformation();

            //таймер
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(configuration.RefreshInterval);
            timer.Tick += TimerTick;
            timer.Start();
        }

        /// <summary>
        /// Обновить конфигурацию
        /// </summary>
        public void ReloadConfiguration()
        {
            ConfigurationManager.Reload();
            configuration = ConfigurationManager.CurrentConfiguration;
            if (service != null)
                service.Settings = configuration.ServiceSettings;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            timer.Interval = TimeSpan.FromSeconds(configuration.RefreshInterval);
            LoadPostsAsync();
        }

        /// <summary>
        /// Загрузить сообщения ассинхронно
        /// </summary>
        public void LoadPostsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            service.LoadLastPostsAsync(configuration.PostLimit, Loaded);
        }

        private void Loaded(OperationCompletedEventArgs<LastMessages> obj)
        {
            IsBusy = false;
            if (IsFirstLoading)
                IsFirstLoading = false;

            if (obj.Error == null)
                try
                {
                    posts = FilterPosts(obj.Result.Posts).ToObservable();
                    IsError = false;
                }
                catch (Exception exc)
                {
                    IsError = true;
                    Logger.LogError(exc, "Error during LastMessages data loading");
                }
            else
            {
                IsError = true;
                ErrorMessage = obj.Error.Message.Length > 35 ? obj.Error.Message.Remove(35) : obj.Error.Message;
                Logger.LogError(obj.Error, "Error during LastMessages data loading");
            }

            if (SelectedPost == null)
                Posts = posts; //update only when there is no selected item
        }

        private IEnumerable<Post> FilterPosts(IEnumerable<Post> posts)
        {
            if (posts == null)
                return new List<Post>(0);

            var result = new List<Post>(posts);

            try
            {
                if (!string.IsNullOrEmpty(configuration.IgnorePosts))
                {
                    var ignoreItems = configuration.IgnorePosts.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(i => i.Trim().ToLower()).Where(i => !string.IsNullOrEmpty(i));
                    if (ignoreItems.Count() > 0)
                        foreach (var post in posts)
                            if (ignoreItems.Any(i => post.PageText.ToLower().Contains(i) || post.Thread.Title.ToLower().Contains(i)))
                                result.Remove(post);
                }

                if (!string.IsNullOrEmpty(configuration.IgnoreNicks))
                {
                    var ignoreItems = configuration.IgnoreNicks.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(i => i.Trim()).Where(i => !string.IsNullOrEmpty(i));
                    if (ignoreItems.Count() > 0)
                        foreach (var post in posts)
                            if (ignoreItems.Any(i => post.User.UserName.Equals(i, StringComparison.CurrentCultureIgnoreCase)))
                                result.Remove(post);
                }
                return result;
            }
            catch
            {
                return result;
            }
        }
    }
}