using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uruchie.ForumGadjet.Model;

namespace Uruchie.ForumGadjet.ViewModel
{
    partial class PostsViewModel
	{
        /// <summary>
        /// Выделенный пост
        /// </summary>
        public Post SelectedPost
        {
            get { return selectedPost; }
            set
            {
                selectedPost = value;
                if (value == null)
                    Posts = posts; //update the post list when deselected
                RaisePropertyChanged("SelectedPost");
            }
        }

        /// <summary>
        /// Список постов
        /// </summary>
        public List<Post> Posts
        {
            get { return posts; }
            set
            {
                posts = value;

                if (IsInDesignMode && value != null && value.Count > 0)
                    SelectedPost = value[0];

                RaisePropertyChanged("Posts");
            }
        }

        /// <summary>
        /// Первый раз загружаемся?
        /// </summary>
        public bool IsFirstLoading
        {
            get { return isFirstLoading; }
            set
            {
                isFirstLoading = value;
                RaisePropertyChanged("IsFirstLoading");
            }
        }

        /// <summary>
        /// Есть ошибки?
        /// </summary>
        public bool IsError
        {
            get { return isError; }
            set
            {
                isError = value;
                RaisePropertyChanged("IsError");
            }
        }

	}
}
