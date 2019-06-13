using System;
using System.Globalization;
using System.IO;

namespace NGettext.Wpf
{
    public interface ILocalizer
    {
        ICatalog Catalog { get; }
        ICultureTracker CultureTracker { get; }

        ICatalog GetCatalog(CultureInfo cultureInfo);
    }

    public class Localizer : IDisposable, ILocalizer
    {
        private readonly string _domainName;

        public Localizer(ICultureTracker cultureTracker, string domainName)
        {
            _domainName = domainName;
            CultureTracker = cultureTracker;
            if (cultureTracker == null) throw new ArgumentNullException(nameof(cultureTracker));
            cultureTracker.CultureChanging += ResetCatalog;
            ResetCatalog(cultureTracker.CurrentCulture);
        }

        private void ResetCatalog(object sender, CultureEventArgs e)
        {
            ResetCatalog(e.CultureInfo);
        }

        private void ResetCatalog(CultureInfo cultureInfo)
        {
            Catalog = GetCatalog(cultureInfo);
        }

        public ICatalog GetCatalog(CultureInfo cultureInfo)
        {
            var localeDir = "Locale";
            Console.WriteLine(
                $"NGettext.Wpf: Attempting to load \"{Path.GetFullPath(Path.Combine(localeDir, cultureInfo.Name, "LC_MESSAGES", _domainName + ".mo"))}\"");
            return new Catalog(_domainName, localeDir, cultureInfo);
        }

        public ICatalog Catalog { get; private set; }
        public ICultureTracker CultureTracker { get; }

        public void Dispose()
        {
            CultureTracker.CultureChanging -= ResetCatalog;
        }
    }

    public static class LocalizerExtensions
    {
        internal struct MsgIdWithContext
        {
            internal string Context { get; set; }
            internal string MsgId { get; set; }
        }

        internal static MsgIdWithContext ConvertToMsgIdWithContext(string msgId)
        {
            var result = new MsgIdWithContext { MsgId = msgId };

            if (msgId.Contains("|"))
            {
                var pipePosition = msgId.IndexOf('|');
                result.Context = msgId.Substring(0, pipePosition);
                result.MsgId = msgId.Substring(pipePosition + 1);
            }

            return result;
        }

        internal static string Gettext(this ILocalizer @this, string msgId, params object[] values)
        {
            if (msgId is null) return null;

            var msgIdWithContext = ConvertToMsgIdWithContext(msgId);

            if (@this is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return string.Format(msgIdWithContext.MsgId, values);
            }

            if (msgIdWithContext.Context != null)
            {
                return @this.Catalog.GetParticularString(msgIdWithContext.Context, msgIdWithContext.MsgId, values);
            }
            return @this.Catalog.GetString(msgIdWithContext.MsgId, values);
        }

        internal static string Gettext(this ILocalizer @this, string msgId)
        {
            if (msgId is null) return null;

            var msgIdWithContext = ConvertToMsgIdWithContext(msgId);

            if (@this is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return msgIdWithContext.MsgId;
            }

            if (msgIdWithContext.Context != null)
            {
                return @this.Catalog.GetParticularString(msgIdWithContext.Context, msgIdWithContext.MsgId);
            }
            return @this.Catalog.GetString(msgIdWithContext.MsgId);
        }
    }
}