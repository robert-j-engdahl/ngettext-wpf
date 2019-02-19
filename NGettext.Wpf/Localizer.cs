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
}