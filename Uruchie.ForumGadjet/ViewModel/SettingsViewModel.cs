using System;
using Uruchie.ForumGadjet.Helpers.Mvvm;
using Uruchie.ForumGadjet.Settings;
using Uruchie.ForumGadjet.Skins;

namespace Uruchie.ForumGadjet.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly string previousSkin;
        private int refreshInterval;
        private string selectedSkin;
        private string[] skins;

        public SettingsViewModel()
        {
            ApplyChangesCommand = new RelayCommand(ApplyChanges, Validate);
            Skins = SkinManager.GetSkins();

            //copy settings:
            RefreshInterval = ConfigurationManager.CurrentConfiguration.RefreshInterval;
            SelectedSkin = previousSkin = ConfigurationManager.CurrentConfiguration.Skin;
        }

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

        public string SelectedSkin
        {
            get { return selectedSkin; }
            set
            {
                selectedSkin = value;
                RaisePropertyChanged("SelectedSkin");
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
            Configuration config = ConfigurationManager.CurrentConfiguration;
            config.RefreshInterval = RefreshInterval;
            config.Skin = SelectedSkin;
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