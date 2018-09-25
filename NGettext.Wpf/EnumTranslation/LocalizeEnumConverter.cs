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
                CompositionRoot.WriteMissingInitializationErrorMessage();
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

        [Obsolete("This public property will be removed in 1.1")]
        public static IEnumLocalizer EnumLocalizer { get; set; }
    }
}