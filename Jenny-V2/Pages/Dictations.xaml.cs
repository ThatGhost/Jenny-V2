using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Jenny_V2.Pages
{
    /// <summary>
    /// Interaction logic for Dictations.xaml
    /// </summary>
    public partial class DictationsPage : Page
    {
        private readonly MainWindow _mainWindow;
        private readonly SpeechRecognizerService _speechRecognizerService;

        public delegate void AddToList(string text);
        public static AddToList addToList;

        public DictationsPage(
            MainWindow mainWindow,
            SpeechRecognizerService speechRecognizerService
            )
        {
            _mainWindow = mainWindow;
            _speechRecognizerService = speechRecognizerService;

            InitializeComponent();
            addToList += AddToDictationList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Navigate<MainPage>();
            _speechRecognizerService.AutoAwnser = true;
        }

        private void AddToDictationList(string text)
        {
            LstDictation.Items.Add(text);
        }
    }
}
