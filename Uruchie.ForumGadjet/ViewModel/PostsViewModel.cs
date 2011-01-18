using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using Uruchie.ForumGadjet.Helpers;
using Uruchie.ForumGadjet.Helpers.Mvvm;
using Uruchie.ForumGadjet.Model;
using Uruchie.ForumGadjet.Service;
using Uruchie.ForumGadjet.Settings;
using Uruchie.ForumGadjet.Skins;

namespace Uruchie.ForumGadjet.ViewModel
{
    /// <summary>
    /// ViewModel for Posts View
    /// </summary>
    public partial class PostsViewModel : ViewModelBase
    {
        private readonly DispatcherTimer timer;
        private string currentSkin;
        private Configuration configuration;
        private bool isError;
        private bool isFirstLoading;
        private List<Post> posts;
        private string postsQuery;
        private Post selectedPost;


        public PostsViewModel()
        {
            ReloadConfiguration();
            InitializeCommands();

            if (!IsInDesignMode)
                IsFirstLoading = true;

            LoadPostsAsync(configuration.PostLimit);

            if (IsInDesignMode)
                return;

            CurrentSkin = SkinManager.DefaultSkin;
            Logger.LogDebug(string.Format("[Started at {0}. Installed .NET versions: {1};]",
                                            DateTime.Now, SystemInfoHelper.GetInstalledDotNetVersions()));

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
            if (IsInDesignMode)
            {
                postsQuery = "module=forum&action=lastmessages&limit=20";
                configuration = ConfigurationManager.Load();
                return;
            }


            configuration = ConfigurationManager.Load();
            Logger.RegisterUrl(configuration.ApiUrl);

            // filter is for loading of only necessary fields (reduces traffic)
            postsQuery = string.Format("module=forum&action=lastmessages&limit={0}&filter={1}",
                                       configuration.PostLimit,
                                       ReflectionHelper.GetActiveDataMembers(typeof (Post), typeof (User), typeof (Thread)));
        }

        private void TimerTick(object sender, EventArgs e)
        {
            LoadPostsAsync(configuration.PostLimit);
        }

        /// <summary>
        /// Загрузить сообщения ассинхронно
        /// </summary>
        public void LoadPostsAsync(int maxPostCount)
        {
            IsError = false;
            IsBusy = true;
            UruchieForumService.LoadDataAsync<LastMessages>(configuration.ApiUrl, postsQuery, Loaded);
        }

        private void Loaded(OperationCompletedEventArgs<LastMessages> obj)
        {
            if (IsFirstLoading)
                IsFirstLoading = false;

            if (obj.Error == null)
                try
                {
                    posts = DataProcessor.PreparePosts(obj.Result.Posts, configuration).ToList();
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

            IsBusy = false;
        }
    }
}