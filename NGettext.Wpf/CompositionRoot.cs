using NGettext.Wpf.EnumTranslation;

namespace NGettext.Wpf
{
    public class CompositionRoot
    {
        public static void Compose(string domainName, NGettextWpfDependencyResolver dependencyResolver = null)
        {
            if (dependencyResolver is null) dependencyResolver = new NGettextWpfDependencyResolver();

            var cultureTracker = dependencyResolver.ResolveCultureTracker();
            var localizer = new Localizer(cultureTracker, domainName);

            ChangeCultureCommand.CultureTracker = cultureTracker;
            GettextExtension.Localizer = localizer;
            TrackCurrentCultureBehavior.CultureTracker = cultureTracker;
            LocalizeEnumConverter.EnumLocalizer = new EnumLocalizer(localizer);
            Translation.Localizer = localizer;
        }
    }
}