using System.Windows;

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
            GettextExtension.Localizer = new Localizer(cultureTracker, "Example");
            TrackCurrentCultureBehavior.CultureTracker = cultureTracker;
        }
    }
}