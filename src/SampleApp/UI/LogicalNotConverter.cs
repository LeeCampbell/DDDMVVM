using System;
using System.Globalization;
using System.Windows.Data;

namespace SampleApp.UI
{
    public sealed class LogicalNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = (bool) value;
            return !flag;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = (bool)value;
            return !flag;
        }
    }
}