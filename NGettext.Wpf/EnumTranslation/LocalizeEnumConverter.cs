using System;
using System.Globalization;
using System.Windows.Data;

namespace NGettext.Wpf.EnumTranslation
{
    public class LocalizeEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumLocalizer.LocalizeEnum((Enum)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static IEnumLocalizer EnumLocalizer { get; set; }
    }
}