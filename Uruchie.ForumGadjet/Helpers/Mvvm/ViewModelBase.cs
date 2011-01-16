using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace Uruchie.ForumGadjet.Helpers.Mvvm
{
    /// <summary>
    /// Base class for all ViewModel's
    /// </summary>
    public class ViewModelBase : PropertyChangedBase
    {
        private static bool? isInDesignMode;
        private bool isBusy;

        /// <summary>
        /// Check if executed in design time
        /// </summary>
        public bool IsInDesignMode
        {
            get { return IsInDesignModeStatic; }
        }

        /// <summary>
        /// Check if executed in design time (static)
        /// </summary>
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!isInDesignMode.HasValue)
                {
                    isInDesignMode = new bool?((bool)
                                               DependencyPropertyDescriptor.FromProperty(
                                                   DesignerProperties.IsInDesignModeProperty,
                                                   typeof (FrameworkElement)).Metadata.DefaultValue);
                    if (
                        !(isInDesignMode.Value ||
                          !Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal)))
                    {
                        isInDesignMode = true;
                    }
                }
                return isInDesignMode.Value;
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }
    }
}