using System;
using System.Windows;
using Uruchie.Core.Helpers;
using Uruchie.ForumGadjet.ViewModel;

namespace Uruchie.ForumGadjet.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        private readonly SettingsViewModel viewModel;

        public SettingsView()
        {
            InitializeComponent();
            DataContext = viewModel = new SettingsViewModel();
            viewModel.AppliedChanges += (s, e) => DialogResult = true;
            viewModel.BeforeSubmit += ViewModelBeforeSubmit;
        }

        private void ViewModelBeforeSubmit(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(passwordBox.Password))
                viewModel.PasswordHash = string.Empty;
            else
                viewModel.PasswordHash = CryptoHelper.CalculateMd5(passwordBox.Password);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (DialogResult == false) //because it is nullable :)
                viewModel.CancelChanges();

            base.OnClosed(e);
        }
    }
}