using System.Windows;

namespace NGettext.Wpf.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Localizer.Noop("Danish");
            Localizer.Noop("English");
            Localizer.Noop("German");
            Localizer.Noop("NGettext.WPF Example");
            InitializeComponent();
        }
    }
}