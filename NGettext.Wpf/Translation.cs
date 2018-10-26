using System;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;

namespace NGettext.Wpf
{
    public static class Translation
    {
        [StringFormatMethod("msgId")]
        public static string _(string msgId, params object[] @params)
        {
            if (Localizer is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return (@params.Any() ? string.Format(CultureInfo.InvariantCulture, msgId, @params) : msgId);
            }
            return @params.Any() ? Localizer.Catalog.GetString(msgId, @params) : Localizer.Catalog.GetString(msgId);
        }

        public static ILocalizer Localizer { get; set; }

        public static string Noop(string msgId) => msgId;

        [StringFormatMethod("singularMsgId")]
        [StringFormatMethod("pluralMsgId")] //< not yet supported, #1833369.
        public static string PluralGettext(int n, string singularMsgId, string pluralMsgId, params object[] @params)
        {
            if (Localizer is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return string.Format(CultureInfo.InvariantCulture, n == 1 ? singularMsgId : pluralMsgId, @params);
            }

            return @params.Any()
                ? Localizer.Catalog.GetPluralString(singularMsgId, pluralMsgId, n, @params)
                : Localizer.Catalog.GetPluralString(singularMsgId, pluralMsgId, n);
        }
    }
}