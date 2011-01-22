using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using Uruchie.ForumGadjet.Helpers;
using Uruchie.ForumGadjet.Helpers.Mvvm;
using Uruchie.ForumGadjet.Model;
using Uruchie.ForumGadjet.Service;
using Uruchie.ForumGadjet.Settings;

namespace Uruchie.ForumGadjet.ViewModel
{
    /// <summary>
    /// ViewModel for Posts View
    /// </summary>
    public partial class PostsViewModel : ViewModelBase
    {
        private readonly DispatcherTimer timer;
        private Configuration configuration;
        private string currentSkin;
        private bool isError;
        private bool isFirstLoading;
        private List<Post> posts;
        private Post selectedPost;


        public PostsViewModel()
        {
            ReloadConfiguration();
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
        }

        private void TimerTick(object sender, EventArgs e)
        {
            timer.Interval = TimeSpan.FromSeconds(ConfigurationManager.CurrentConfiguration.RefreshInterval);
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
            UruchieForumService.LoadLastPostsAsync(Loaded);
        }

        private void Loaded(OperationCompletedEventArgs<LastMessages> obj)
        {
            IsBusy = false;
            if (IsFirstLoading)
                IsFirstLoading = false;

            if (obj.Error == null)
                try
                {
                    posts = DataProcessor.PreparePosts(obj.Result.Posts, configuration).ToList();
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
                Logger.LogError(obj.Error, "Error during LastMessages data loading");
            }

            if (SelectedPost == null)
                Posts = posts; //update only when there is no selected item
        }
    }
}