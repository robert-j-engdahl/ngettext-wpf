using System.Windows;

namespace NGettext.Wpf.Example
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            CompositionRoot.Compose("Example");
            base.OnStartup(e);
        }
    }
}