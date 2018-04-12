using System.Windows;
using NGettext.Wpf.EnumTranslation;

namespace NGettext.Wpf.Example
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            CompositionRoot();
            base.OnStartup(e);
        }

        private static void CompositionRoot()
        {
            var cultureTracker = new CultureTracker();
            ChangeCultureCommand.CultureTracker = cultureTracker;
            var localizer = new Localizer(cultureTracker, "Example");
            GettextExtension.Localizer = localizer;
            TrackCurrentCultureBehavior.CultureTracker = cultureTracker;
            LocalizeEnumConverter.EnumLocalizer = new EnumLocalizer(localizer);
            Translation.Localizer = localizer;
        }
    }
}