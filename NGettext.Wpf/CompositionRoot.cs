using NGettext.Wpf.Common;
using NGettext.Wpf.EnumTranslation;

namespace NGettext.Wpf
{
    public static class CompositionRoot
    {
        public static void Compose(string domainName, string localeDir = "Locale", NGettextWpfDependencyResolver dependencyResolver = null)
        {
            if (dependencyResolver is null) dependencyResolver = new NGettextWpfDependencyResolver();

            var cultureTracker = dependencyResolver.ResolveCultureTracker();
            var localizer = new Localizer(cultureTracker, domainName, localeDir);

            ChangeCultureCommand.CultureTracker = cultureTracker;
            GettextExtension.Localizer = localizer;
            TrackCurrentCultureBehavior.CultureTracker = cultureTracker;
            LocalizeEnumConverter.EnumLocalizer = new EnumLocalizer(localizer);
            Translation.Localizer = localizer;
            GettextStringFormatConverter.Localizer = localizer;
        }

        internal static void WriteMissingInitializationErrorMessage()
        {
            System.Console.Error.WriteLine("NGettext.Wpf: NGettext.Wpf.CompositionRoot.Compose() must be called at the entry point of the application for localization to work");
        }
    }
}