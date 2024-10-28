using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Jenny_V2.Services;
using Jenny_V2.Services.ResearchContext;

namespace Jenny_V2.Pages
{
    public partial class ResearchContextChat : Page, IPageLifeTime
    {
        private readonly ResearchChatService _researchChatService;
        private readonly ChatGPTService _chatGPTService;
        private readonly MainWindow _mainWindow;
        private readonly SpeechRecognizerService _speechRecognizerService;
        private bool _speechRecognizerEnabled = false;

        public ResearchContextChat(
            ResearchChatService researchChatService,
            ChatGPTService chatGPTService,
            MainWindow mainWindow,
            SpeechRecognizerService speechRecognizerService
            )
        {
            _chatGPTService = chatGPTService;
            _researchChatService = researchChatService;
            _mainWindow = mainWindow;
            _speechRecognizerService = speechRecognizerService;

            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Navigate<MainPage>();
        }

        private void BtnMicrophone_Click(object sender, RoutedEventArgs e)
        {
            if(_speechRecognizerEnabled)
            {
                _speechRecognizerEnabled = false;
                _speechRecognizerService.AutoAwnser = true;
                _speechRecognizerService.onSpeechRegognized -= OnRegognizedInput;
                IconMicrophone.Icon = FontAwesome.WPF.FontAwesomeIcon.MicrophoneSlash;
                IconMicrophone.Foreground = Brushes.Black;
            }
            else
            {
                _speechRecognizerEnabled = true;
                _speechRecognizerService.AutoAwnser = false;
                _speechRecognizerService.onSpeechRegognized += OnRegognizedInput;
                IconMicrophone.Icon = FontAwesome.WPF.FontAwesomeIcon.Microphone;
                IconMicrophone.Foreground = Brushes.LightBlue;
            }
        }

        private void OnRegognizedInput(string text)
        {
            Application.Current.Dispatcher.Invoke(() => TxtBoxInput.Text += text);
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            _chatGPTService.GetAiResponse(TxtBoxInput.Text);

            Application.Current.Dispatcher.Invoke(() =>
            {
                var paragraph = new Paragraph(new Run(TxtBoxInput.Text));
                StyleParagraphe(paragraph, true);
                RichTxtBoxChat.Document.Blocks.Add(paragraph);
                TxtBoxInput.Text = "";
            });
        }

        private void OnAiResponse(string awnser)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var paragraph = new Paragraph(new Run(awnser));
                StyleParagraphe(paragraph, false);
                RichTxtBoxChat.Document.Blocks.Add(paragraph);
            });
        }

        private void StyleParagraphe(Paragraph paragraph, bool sendByUser)
        {
            BrushConverter bc = new BrushConverter();
            paragraph.TextAlignment = sendByUser ? TextAlignment.Right : TextAlignment.Left;
            paragraph.Foreground = Brushes.Black;
            paragraph.Background = sendByUser ? Brushes.CornflowerBlue : Brushes.LightGray;
            paragraph.Padding = new Thickness(8);
            paragraph.BorderBrush = Brushes.Black;
            paragraph.BorderThickness = new Thickness(1);
            paragraph.Margin = new Thickness(sendByUser ? 50 : 5 , 5, sendByUser ? 5 : 50, 5);
        }

        void IPageLifeTime.OnPageEnter()
        {
            _chatGPTService.AutoSpeak = false;
            _chatGPTService.onAIResponse += OnAiResponse;
        }

        void IPageLifeTime.OnPageExit()
        {
            _chatGPTService.AutoSpeak = true;
            _chatGPTService.onAIResponse -= OnAiResponse;
        }
    }
}
