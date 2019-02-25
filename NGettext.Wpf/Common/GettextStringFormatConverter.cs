using System;
using System.Globalization;
using System.Windows.Data;

namespace NGettext.Wpf.Common
{
    public class GettextStringFormatConverter : IValueConverter
    {
        private readonly string _msgId;

        public GettextStringFormatConverter(string msgId)
        {
            _msgId = msgId;
        }

        public static ILocalizer Localizer { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Localizer.Gettext(_msgId, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}