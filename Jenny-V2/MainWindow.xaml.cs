using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Jenny_V2.Services;

namespace Jenny_V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SpeechRecognizerService _speechRecognizerService;

        public delegate void Log(string log);
        public delegate void Toggle(bool on);
        public static Log onLog;
        public static Log onJenny;
        public static Log onUser;
        public static Toggle onToggleLight;

        public MainWindow(
            SpeechRecognizerService speechRecognizerService
            )
        {
            _speechRecognizerService = speechRecognizerService;

            onLog += LogOnWindow;
            onJenny += JennyOnWindow;
            onUser += UserOnWindow;
            onToggleLight += toggleLight;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _speechRecognizerService.ToggleSpeechRegonition();
        }

        private void ClosingWindow(object sender, CancelEventArgs e)
        {
            _speechRecognizerService.StopSpeechRegonition();
            _speechRecognizerService.Dispose();
        }

        private void LogOnWindow(string text)
        {
            if (text == "") return;
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                LstLogs.Items.Insert(0, text);
            });
        }

        private void JennyOnWindow(string text)
        {
            if (text == "") return;
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                LstJennyText.Items.Insert(0, text);
            });
        }

        private void UserOnWindow(string text)
        {
            if (text == "") return;
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                LstSpokenText.Items.Insert(0, text);
            });
        }

        private void toggleLight(bool on)
        {
            BrushConverter bc = new BrushConverter();

            if (on) CircleIsListening.Fill = (Brush)bc.ConvertFrom("Green")!;
            else CircleIsListening.Fill = (Brush)bc.ConvertFrom("Red")!;
        }
    }
}