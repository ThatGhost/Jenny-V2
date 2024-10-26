using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

using Jenny_V2.Pages;
using Jenny_V2.Services;

namespace Jenny_V2
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SpeechRecognizerService _speechRecognizerService;
        public MainWindow(
            MainPage MainPage,
            SpeechRecognizerService speechRecognizerService
            )
        {
            _speechRecognizerService = speechRecognizerService;

            InitializeComponent();
            MainFrame.Navigate(MainPage);
        }

        private void ClosingWindow(object sender, CancelEventArgs e)
        {
            _speechRecognizerService.StopSpeechRegonition();
            _speechRecognizerService.Dispose();
        }
    }
}
