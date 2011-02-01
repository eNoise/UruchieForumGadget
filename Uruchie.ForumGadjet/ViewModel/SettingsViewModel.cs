using System;
using Uruchie.Core.Presentation;
using Uruchie.ForumGadjet.Settings;
using Uruchie.ForumGadjet.Skins;

namespace Uruchie.ForumGadjet.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly bool isInitializing = true;
        private readonly string previousSkin;
        private string ignoreNicks;
        private string ignorePosts;
        private string passwordHash;
        private int refreshInterval;
        private string userName;
        private string selectedSkin;
        private string[] skins;

        public SettingsViewModel()
        {
            ApplyChangesCommand = new RelayCommand(ApplyChanges, Validate);
            Skins = SkinManager.GetSkins();

            //copy settings:
            var cfg = ConfigurationManager.CurrentConfiguration;
            RefreshInterval = cfg.RefreshInterval;
            SelectedSkin = previousSkin = cfg.Skin;
            IgnoreNicks = cfg.IgnoreNicks;
            IgnorePosts = cfg.IgnorePosts;
            UserName = cfg.ServiceSettings.UserName;
            PasswordHash = "11111111";

            isInitializing = false;
        }

        public event EventHandler BeforeSubmit = delegate { }; 

        public RelayCommand ApplyChangesCommand { get; set; }

        public int RefreshInterval
        {
            get { return refreshInterval; }
            set
            {
                refreshInterval = value;
                RaisePropertyChanged("RefreshInterval");
            }
        }

        public string[] Skins
        {
            get { return skins; }
            set
            {
                skins = value;
                RaisePropertyChanged("Skins");
            }
        }

        public string IgnoreNicks
        {
            get { return ignoreNicks; }
            set
            {
                ignoreNicks = value;
                RaisePropertyChanged("IgnoreNicks");
            }
        }

        public string IgnorePosts
        {
            get { return ignorePosts; }
            set
            {
                ignorePosts = value;
                RaisePropertyChanged("IgnorePosts");
            }
        }

        public string PasswordHash
        {
            get { return passwordHash; }
            set
            {
                passwordHash = value;
                RaisePropertyChanged("PasswordHash");
            }
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                RaisePropertyChanged("Username");
            }
        }

        public string SelectedSkin
        {
            get { return selectedSkin; }
            set
            {
                selectedSkin = value;
                RaisePropertyChanged("SelectedSkin");
                if (!isInitializing)
                    SkinManager.ChangeSkin(value);
            }
        }

        public event EventHandler AppliedChanges = delegate { };

        private bool Validate()
        {
            return RefreshInterval >= 10 && RefreshInterval < 1000 &&
                   !string.IsNullOrEmpty(SelectedSkin);
        }

        public void ApplyChanges()
        {
            BeforeSubmit(this, EventArgs.Empty);
            Configuration config = ConfigurationManager.CurrentConfiguration;
            config.RefreshInterval = RefreshInterval;
            config.IgnorePosts = IgnorePosts;
            config.IgnoreNicks = IgnoreNicks;
            config.ServiceSettings.UserName = UserName;
            config.Skin = SelectedSkin;
            config.ServiceSettings.PasswordHash = PasswordHash;
            ConfigurationManager.Save();

            AppliedChanges(this, EventArgs.Empty);
        }

        public void CancelChanges()
        {
            ConfigurationManager.CurrentConfiguration.Skin = previousSkin;
            SkinManager.ChangeSkin(previousSkin);
        }
    }
}