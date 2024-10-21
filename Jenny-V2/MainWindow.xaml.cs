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

using Jenny_V2.EventHandlers.Core;
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
        private readonly KeywordService _keywordService;
        private readonly EventFactory _eventFactory;

        public delegate void Log(string log);
        public static Log onLog;
        public static Log onJenny;
        public static bool AutoAwnser = true;

        public MainWindow(
            SpeechRecognizerService speechRecognizerService, 
            ChatGPTService chatGPTService,
            KeywordService keywordService,
            EventFactory eventFactory
            )
        {
            _speechRecognizerService = speechRecognizerService;
            _chatGPTService = chatGPTService;
            _keywordService = keywordService;
            _eventFactory = eventFactory;

            _speechRecognizerService.SpeechRecognized += OnSpeechRegonised;
            _chatGPTService.onAIResponse += OnAiResponse;
            onLog += LogOnWindow;
            onJenny += JennyOnWindow;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            _speechRecognizerService.ToggleSpeechRegonition();

            if (_speechRecognizerService.IsRegonizing) CircleIsListening.Fill = (Brush)bc.ConvertFrom("Green")!;
            else CircleIsListening.Fill = (Brush)bc.ConvertFrom("Red")!;

            IcnLoadingIcon.Spin = true;
        }

        private void ClosingWindow(object sender, CancelEventArgs e)
        {
            _speechRecognizerService.StopSpeechRegonition();
            _speechRecognizerService.Dispose();
        }

        private void OnSpeechRegonised(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LogOnWindow(text);
                TextCommand? textCommand = _keywordService.FindTextCommand(text);
                LogOnWindow(textCommand.ToString());
                if (textCommand != null) _eventFactory.HandleEvent(textCommand.Value, text);
                else
                {
                    if(text.ToLower().Contains("jenny") && AutoAwnser) 
                        _chatGPTService.GetAiResponse($"your name is jenny.\nCan you respond to the user in a friendly and consise manner?\nuser- '{text}'");
                }
            });
        }

        private void OnAiResponse(string text)
        {
            JennyOnWindow(text);
        }

        private void LogOnWindow(string text)
        {
            if (text == "") return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                LstLogs.Items.Insert(0, text);
            });
        }

        private void JennyOnWindow(string text)
        {
            if (text == "") return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                LstJennyText.Items.Insert(0, text);
            });
        }
    }
}