using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace NGettext.Wpf.Example
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        private int _memoryLeakTestProgress;
        private DateTime _currentTime;

        public MainWindow()
        {
            InitializeComponent();
            Title = GettextExtension.Localizer.Catalog.GetString("NGettext.WPF Example");

            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.1)};
            timer.Tick += (sender, args) => { CurrentTime = DateTime.Now; };
            timer.Start();
        }

        public decimal SomeNumber => 1234567.89m;

        public DateTime CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTime)));
            }
        }

        private void OpenMemoryLeakTestWindow(object sender, RoutedEventArgs e)
        {
            var window = new MemoryLeakTestWindow();
            window.Closed += async (o, args) =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                ++MemoryLeakTestProgress;
                var expectedLanguage = window.Language;
                foreach (var locale in new[]
                    {"da-DK", "de-DE", "en-US", TrackCurrentCultureBehavior.CultureTracker.CurrentCulture.Name})
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    TrackCurrentCultureBehavior.CultureTracker.CurrentCulture = CultureInfo.GetCultureInfo(locale);
                    Debug.Assert(window.Language == expectedLanguage);
                    ++MemoryLeakTestProgress;
                }
            };
            window.Show();
            MemoryLeakTestProgress = 0;

            window.Close();
        }

        public int MemoryLeakTestProgress
        {
            get => _memoryLeakTestProgress;
            set
            {
                _memoryLeakTestProgress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemoryLeakTestProgress)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}