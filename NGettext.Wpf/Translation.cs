using System;
using System.Linq;

namespace NGettext.Wpf
{
    public static class Translation
    {
        public static string _(string msgId, params object[] @params)
        {
            if (Localizer is null) throw new Exception("Translation.Localizer must be initialized");
            return @params.Any() ? Localizer.Catalog.GetString(msgId, @params) : Localizer.Catalog.GetString(msgId);
        }

        public static ILocalizer Localizer { get; set; }
    }
}