using System;
using System.Globalization;
using System.Windows.Data;

namespace NGettext.Wpf.EnumTranslation
{
    public class LocalizeEnumConverter : IValueConverter
    {
        private IEnumLocalizer _enumLocalizer;

        public LocalizeEnumConverter(IEnumLocalizer enumLocalizer = null)
        {
            _enumLocalizer = enumLocalizer;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumLocalizer = GetEnumLocalizer();
            if (enumLocalizer is null)
            {
                return value;
            }

            if (value is Enum enumValue)
            {
                return enumLocalizer.LocalizeEnum(enumValue);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        [Obsolete("This public property will be removed in 1.1")]
        public static IEnumLocalizer EnumLocalizer { get; set; }

        private IEnumLocalizer GetEnumLocalizer()
        {
            var result = _enumLocalizer ?? EnumLocalizer;

            if (result is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
            }

            return result;
        }
    }
}