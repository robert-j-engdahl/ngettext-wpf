using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using JetBrains.Annotations;
using NGettext.Wpf.Serialization;

namespace NGettext.Wpf
{
    public static class Translation
    {
        private static TranslationSerializer _translationSerializer;

        [StringFormatMethod("msgId")]
        public static string _(string msgId, params object[] @params)
        {
            return @params.Any() ? Localizer.Gettext(msgId, @params) : Localizer.Gettext(msgId);
        }

        public static ILocalizer Localizer { get; set; }

        public static string Noop(string msgId) => msgId;

        [StringFormatMethod("singularMsgId")]
        [StringFormatMethod("pluralMsgId")] //< not yet supported, #1833369.
        [Obsolete("Use GetPluralString() instead.  This method will be removed in 2.x")]
        public static string PluralGettext(int n, string singularMsgId, string pluralMsgId, params object[] @params)
        {
            return GetPluralString(singularMsgId, pluralMsgId, n, @params);
        }

        [StringFormatMethod("singularMsgId")]
        [StringFormatMethod("pluralMsgId")] //< not yet supported, #1833369.
        public static string GetPluralString(string singularMsgId, string pluralMsgId, int n, params object[] args)
        {
            if (Localizer is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return string.Format(CultureInfo.InvariantCulture, n == 1 ? singularMsgId : pluralMsgId, args);
            }

            return args.Any()
                ? Localizer.Catalog.GetPluralString(singularMsgId, pluralMsgId, n, args)
                : Localizer.Catalog.GetPluralString(singularMsgId, pluralMsgId, n);
        }

        [StringFormatMethod("text")]
        [StringFormatMethod("pluralText")] //< not yet supported, #1833369.
        public static string GetParticularPluralString(string context, string text, string pluralText, int n, params object[] args)
        {
            if (Localizer is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return string.Format(CultureInfo.InvariantCulture, n == 1 ? text : pluralText, args);
            }

            return args.Any()
                ? Localizer.Catalog.GetParticularPluralString(context, text, pluralText, n, args)
                : Localizer.Catalog.GetParticularPluralString(context, text, pluralText, n);
        }

        [StringFormatMethod("text")]
        public static string GetParticularString(string context, string text, params object[] args)
        {
            if (Localizer is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return (args.Any() ? string.Format(CultureInfo.InvariantCulture, text, args) : text);
            }
            return args.Any() ? Localizer.Catalog.GetParticularString(context, text, args) : Localizer.Catalog.GetParticularString(context, text);
        }

#if ALPHA

        [StringFormatMethod("msgId")]
        [Obsolete("This method is experimental, and may go away")]
        public static string SerializedGettext(IEnumerable<CultureInfo> cultureInfos, string msgId, params object[] args)
        {
            if (Localizer is null)
            {
                var message = args.Any() ? Localizer.Gettext(msgId, args) : Localizer.Gettext(msgId);

                return "{" + string.Join(", ", cultureInfos.Select(c => $"\"{c.Name}\": \"{HttpUtility.JavaScriptStringEncode(message)}\"")) + "}";
            }

            if (_translationSerializer == null)
            {
                _translationSerializer = new TranslationSerializer(Localizer.GetCatalog);
            }

            return _translationSerializer.SerializedGettext(cultureInfos, msgId, args);
        }
#endif
    }
}