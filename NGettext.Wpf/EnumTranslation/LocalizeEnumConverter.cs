using System;
using System.Globalization;
using System.Windows.Data;

namespace NGettext.Wpf.EnumTranslation
{
    public class LocalizeEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (EnumLocalizer is null)
            {
                Console.Error.WriteLine("LocalizeEnumConverter.EnumLocalizer was not initialized.  Localization disabled.");
                return value;
            }

            if (value is Enum enumValue)
            {
                return EnumLocalizer.LocalizeEnum(enumValue);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static IEnumLocalizer EnumLocalizer { get; set; }
    }
}