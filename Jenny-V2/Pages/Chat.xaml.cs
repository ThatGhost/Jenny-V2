using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Jenny_V2.Services;
using Jenny_V2.Services.UI;

namespace Jenny_V2.Pages
{
    public partial class ChatPage : Page, IPageLifeTime
    {
        private readonly ChatService _chatService;
        private readonly MainWindow _mainWindow;
        private readonly SpeechRecognizerService _speechRecognizerService;
        private readonly ChatPageService _chatPageService;
        private bool _speechRecognizerEnabled = false;

        public ChatPage(
            ChatService researchChatService,
            ChatGPTService chatGPTService,
            MainWindow mainWindow,
            SpeechRecognizerService speechRecognizerService,
            ChatPageService chatPageService
            )
        {
            _chatService = researchChatService;
            _mainWindow = mainWindow;
            _speechRecognizerService = speechRecognizerService;
            _chatPageService = chatPageService;

            _chatPageService.OnMessageReceived += MessageReceived;
            _chatPageService.OnShowUserMessage += ShowUserMessage;

            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Navigate<MainPage>();
        }

        private void BtnMicrophone_Click(object sender, RoutedEventArgs e)
        {
            if (_speechRecognizerEnabled)
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
            _chatService.SendChat(TxtBoxInput.Text);
            ShowUserMessage(TxtBoxInput.Text);
            TxtBoxInput.Text = "";
        }

        private void StyleParagraphe(Paragraph paragraph, bool sendByUser)
        {
            BrushConverter bc = new();
            paragraph.TextAlignment = sendByUser ? TextAlignment.Right : TextAlignment.Left;
            paragraph.Foreground = Brushes.Black;
            paragraph.Background = sendByUser ? Brushes.CornflowerBlue : Brushes.LightGray;
            paragraph.Padding = new Thickness(8);
            paragraph.BorderBrush = Brushes.Black;
            paragraph.BorderThickness = new Thickness(1);
            paragraph.Margin = new Thickness(sendByUser ? 50 : 5, 5, sendByUser ? 5 : 50, 5);
        }

        private void MessageReceived(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var paragraph = new Paragraph(new Run(message));
                StyleParagraphe(paragraph, false);
                RichTxtBoxChat.Document.Blocks.Add(paragraph);
            });
        }

        private void ShowUserMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var paragraph = new Paragraph(new Run(message));
                StyleParagraphe(paragraph, true);
                RichTxtBoxChat.Document.Blocks.Add(paragraph);
            });
        }

        void IPageLifeTime.OnPageEnter() => _chatService.Start();
        void IPageLifeTime.OnPageExit() => _chatService.Stop();
    }
}
