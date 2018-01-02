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
            InitializeComponent();
            Title = GettextExtension.Localizer.Catalog.GetString("NGettext.WPF Example");
        }
    }
}