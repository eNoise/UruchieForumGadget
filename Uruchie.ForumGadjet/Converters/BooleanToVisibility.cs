using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Uruchie.ForumGadjet.Converters
{
    public class BooleanToVisibility : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && parameter.ToString() == "False") //it's not a boolean type :)
                return ((bool) value) ? Visibility.Hidden : Visibility.Visible;

            return ((bool) value) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("The operation is not supported.");
        }

        #endregion
    }
}