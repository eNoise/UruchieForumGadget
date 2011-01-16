using System;
using System.Globalization;
using System.Windows.Data;

namespace Uruchie.ForumGadjet.Converters
{
    public class IsNullConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("The operation is not supported.");
        }

        #endregion
    }
}