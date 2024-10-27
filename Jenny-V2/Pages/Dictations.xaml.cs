using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;

using Jenny_V2.Services;

namespace Jenny_V2.Pages
{
    public partial class DictationsPage : Page, IPageNavigatedTo
    {
        private readonly DictationService _dictationService;
        private readonly MainWindow _mainWindow;

        public DictationsPage(
            DictationService dictationService,
            MainWindow mainWindow
            )
        {
            _dictationService = dictationService;
            _mainWindow = mainWindow;

            InitializeComponent();
            _dictationService.OnDictactionHeard += AddToDictationList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _dictationService.Stop();
            _dictationService.RemoveDictiationKeywords();

            _mainWindow.Navigate<MainPage>();
        }

        private void AddToDictationList(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TxtBoxDictation.Document.Blocks.Clear();
                List<string> paragraphes = text.Split("$$").ToList();
                foreach ( var paragrapheString in paragraphes )
                {
                    var paragraphe = new Paragraph(new Run(paragrapheString));
                    paragraphe.FontWeight = paragrapheString.StartsWith("#") ? FontWeights.Bold : FontWeights.Normal;

                    TxtBoxDictation.Document.Blocks.Add(paragraphe);
                }
            });
        }

        private void ToggleDictation_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();

            if (_dictationService.IsDictating) _dictationService.Stop();
            else _dictationService.Start();

            UpdateIsListeningUI();
        }

        private void UpdateIsListeningUI()
        {
            BrushConverter bc = new BrushConverter();

            if (_dictationService.IsDictating)
            {
                IsListening.Fill = (Brush)bc.ConvertFrom("green")!;
                btnToggleDictation.Content = "Stop Dictation";
            }
            else
            {
                IsListening.Fill = (Brush)bc.ConvertFrom("red")!;
                btnToggleDictation.Content = "Resume Dictation";
            }
        }

        void IPageNavigatedTo.OnPageNavigatedTo()
        {
            _dictationService.AddDictiationKeywords();
            _dictationService.Start();

            UpdateIsListeningUI();
        }
    }
}
