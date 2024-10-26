using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly SpeechRecognizerService _speechRecognizerService;

        public delegate void Log(string log);
        public delegate void Toggle(bool on);
        public static Log onLog;
        public static Log onJenny;
        public static Log onUser;
        public static Toggle onToggleLight;

        public MainPage(
            SpeechRecognizerService speechRecognizerService
            )
        {
            InitializeComponent();
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
