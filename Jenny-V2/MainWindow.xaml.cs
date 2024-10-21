using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Jenny_V2.EventHandlers.Core;
using Jenny_V2.Services;

using static System.Net.Mime.MediaTypeNames;

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
        private readonly TextToSpeechService _textToSpeechService;

        public delegate void Log(string log);
        public static Log onLog;
        public static Log onJenny;
        public static Log onLight;
        public static bool AutoAwnser = true;

        public MainWindow(
            SpeechRecognizerService speechRecognizerService, 
            ChatGPTService chatGPTService,
            KeywordService keywordService,
            EventFactory eventFactory,
            TextToSpeechService textToSpeechService
            )
        {
            _speechRecognizerService = speechRecognizerService;
            _chatGPTService = chatGPTService;
            _keywordService = keywordService;
            _eventFactory = eventFactory;
            _textToSpeechService = textToSpeechService;

            _speechRecognizerService.SpeechRecognized += OnSpeechRegonised;
            _chatGPTService.onAIResponse += OnAiResponse;
            onLog += LogOnWindow;
            onJenny += JennyOnWindow;
            onLight += OnLight;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            _speechRecognizerService.ToggleSpeechRegonition();

            if (_speechRecognizerService.IsRegonizing) CircleIsListening.Fill = (Brush)bc.ConvertFrom("Green")!;
            else CircleIsListening.Fill = (Brush)bc.ConvertFrom("Red")!;
        }

        private void ClosingWindow(object sender, CancelEventArgs e)
        {
            _speechRecognizerService.StopSpeechRegonition();
            _speechRecognizerService.Dispose();
        }

        private void OnSpeechRegonised(string text)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (text.Trim() == "") return;
                LstSpokenText.Items.Insert(0,text);
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
            _textToSpeechService.Speak(text);
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

        private void OnLight(string text)
        {
            BrushConverter bc = new BrushConverter();
            if (text == "on") CircleIsListening.Fill = (Brush)bc.ConvertFrom("Green")!;
            else CircleIsListening.Fill = (Brush)bc.ConvertFrom("Red")!;
        }
    }
}