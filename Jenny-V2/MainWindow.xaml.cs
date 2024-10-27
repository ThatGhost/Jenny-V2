using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Jenny_V2.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace Jenny_V2
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(
            IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider; 

            InitializeComponent();
            Navigate<MainPage>();
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
