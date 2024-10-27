using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

using Jenny_V2.Pages;
using Jenny_V2.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Jenny_V2
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SpeechRecognizerService _speechRecognizerService;
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(
            MainPage MainPage,
            SpeechRecognizerService speechRecognizerService,
            IServiceProvider serviceProvider
            )
        {
            _speechRecognizerService = speechRecognizerService;
            _serviceProvider = serviceProvider; 

            InitializeComponent();
            MainFrame.Navigate(MainPage);
        }

        public void Navigate<T>() where T : Page
        {
            Dispatcher.Invoke(() =>
            {
                var page = _serviceProvider.GetRequiredService<T>();
                MainFrame.Navigate(page);

                if (page is IPageNavigatedTo pageNavigation)
                {
                    pageNavigation.OnPageNavigatedTo();
                }
            }, DispatcherPriority.ApplicationIdle);
        }
    }
}
