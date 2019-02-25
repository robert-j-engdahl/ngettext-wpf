using System;
using System.Globalization;
using System.IO;

namespace NGettext.Wpf
{
    public interface ILocalizer
    {
        ICatalog Catalog { get; }
        ICultureTracker CultureTracker { get; }
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
            var localeDir = "Locale";
            Console.WriteLine(
                $"NGettext.Wpf: Attempting to load \"{Path.GetFullPath(Path.Combine(localeDir, cultureInfo.Name, "LC_MESSAGES", _domainName + ".mo"))}\"");
            Catalog = new Catalog(_domainName, localeDir, cultureInfo);
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
        internal static string Gettext(this ILocalizer @this, string msgId, params object[] values)
        {
            string context = null;
            if (msgId.Contains("|"))
            {
                var pipePosition = msgId.IndexOf('|');
                context = msgId.Substring(0, pipePosition);
                msgId = msgId.Substring(pipePosition + 1);
            }

            if (@this is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return string.Format(msgId, values);
            }

            if (context != null)
            {
                return @this.Catalog.GetParticularString(context, msgId, values);
            }
            return @this.Catalog.GetString(msgId, values);
        }

        internal static string Gettext(this ILocalizer @this, string msgId)
        {
            string context = null;
            if (msgId.Contains("|"))
            {
                var pipePosition = msgId.IndexOf('|');
                context = msgId.Substring(0, pipePosition);
                msgId = msgId.Substring(pipePosition + 1);
            }

            if (@this is null)
            {
                CompositionRoot.WriteMissingInitializationErrorMessage();
                return msgId;
            }

            if (context != null)
            {
                return @this.Catalog.GetParticularString(context, msgId);
            }
            return @this.Catalog.GetString(msgId);
        }
    }
}