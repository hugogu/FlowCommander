using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace FlowCommander.Views.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class FullNameToShortNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            return Path.GetFileName(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
