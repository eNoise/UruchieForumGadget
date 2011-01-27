using System.ComponentModel;
using System.Runtime.Serialization;

namespace Uruchie.Core.Presentation
{
    /// <summary>
    /// A simple implementation of an INotifyPropertyChanged interface
    /// </summary>
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        /// <summary>
        /// Raise property changed event for given name
        /// </summary>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}