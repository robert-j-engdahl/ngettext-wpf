using System;
using System.Linq;
using System.Reflection;

namespace NGettext.Wpf
{
    public static class Translation
    {
        public static string _(string msgId, params object[] @params)
        {
            if (Localizer is null)
            {
                Console.Error.WriteLine("NGettext.WPF.Translation.Localizer not set.  Localization is disabled.");
                return (@params.Any() ? string.Format(msgId, @params) : msgId);
            }
            return @params.Any() ? Localizer.Catalog.GetString(msgId, @params) : Localizer.Catalog.GetString(msgId);
        }

        public static ILocalizer Localizer { get; set; }

        public static void Noop(string msgId)
        {
        }
    }
}