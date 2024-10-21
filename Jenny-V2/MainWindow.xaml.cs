using System.ComponentModel;
using System.Reflection.Emit;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Jenny_V2.Services;

namespace Jenny_V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SpeechRecognizerService _speechRecognizerService;
        private readonly ChatGPTService _chatGPTService;
        public MainWindow(
            SpeechRecognizerService speechRecognizerService, ChatGPTService chatGPTService)
        {
            _speechRecognizerService = speechRecognizerService;
            _chatGPTService = chatGPTService;

            _speechRecognizerService.SpeechRecognized += OnSpeechRegonised;
            _chatGPTService.onAIResponse += OnAiResponse;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            _speechRecognizerService.ToggleSpeechRegonition();

            if (_speechRecognizerService.IsRegonizing) CircleIsListening.Fill = (Brush)bc.ConvertFrom("Green")!;
            else CircleIsListening.Fill = (Brush)bc.ConvertFrom("Red")!;

            _chatGPTService.GetAiResponse("is there a project name in this scentence, if yes return it, if no give me the word 'no'\r\n\"Can you make me a new project\"");
        }

        private void ClosingWindow(object sender, CancelEventArgs e)
        {
            _speechRecognizerService.Dispose();
        }

        private void OnSpeechRegonised(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LstSpokenText.Items.Insert(0, text);
            });
        }

        private void OnAiResponse(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LstLogs.Items.Insert(0, text);
            });
        }
    }
}