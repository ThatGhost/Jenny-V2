using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Jenny_V2.Services.ResearchContext;

namespace Jenny_V2.Pages
{
    public partial class DictationsPage : Page, IPageLifeTime
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
            _mainWindow.Navigate<MainPage>();
        }

        private void AddToDictationList(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TxtBoxDictation.Document.Blocks.Clear();
                List<string> paragraphes = text.Split("$$").ToList();
                foreach ( var paragraphString in paragraphes )
                {
                    bool isBold = paragraphString.StartsWith("#");

                    var paragraphe = new Paragraph(new Run(paragraphString.Replace("#", "")));
                    paragraphe.FontWeight = isBold ? FontWeights.Bold : FontWeights.Normal;
                    
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

        void IPageLifeTime.OnPageEnter()
        {
            _dictationService.AddDictiationKeywords();
            _dictationService.Start();

            BrushConverter bc = new BrushConverter();
            IsListening.Fill = (Brush)bc.ConvertFrom("green")!;
            btnToggleDictation.Content = "Stop Dictation";
        }

        void IPageLifeTime.OnPageExit()
        {
            _dictationService.Stop();
            _dictationService.RemoveDictiationKeywords();
        }
    }
}
