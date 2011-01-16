using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Uruchie.ForumGadjet.Converters
{
    public class IsNullToHiddenConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("The operation is not supported.");
        }

        #endregion
    }
}