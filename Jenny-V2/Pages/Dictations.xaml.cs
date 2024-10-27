using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Jenny_V2.Services;

namespace Jenny_V2.Pages
{
    /// <summary>
    /// Interaction logic for Dictations.xaml
    /// </summary>
    public partial class DictationsPage : Page
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
            BrushConverter bc = new BrushConverter();
            IsListening.Fill = (Brush)bc.ConvertFrom("green")!;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _dictationService.Stop();
            _mainWindow.Navigate<MainPage>();
        }

        private void AddToDictationList(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TxtBoxDictation.Document.Blocks.Clear();
                TxtBoxDictation.Document.Blocks.Add(new Paragraph(new Run(text)));
            });
        }

        private void ToggleDictation_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();

            if (_dictationService.IsDictating)
            {
                _dictationService.Stop();
                IsListening.Fill = (Brush)bc.ConvertFrom("red")!;
                btnToggleDictation.Content = "Resume Dictation";
            }
            else
            {
                _dictationService.Start();
                IsListening.Fill = (Brush)bc.ConvertFrom("green")!;
                btnToggleDictation.Content = "Stop Dictation";
            }
        }
    }
}
