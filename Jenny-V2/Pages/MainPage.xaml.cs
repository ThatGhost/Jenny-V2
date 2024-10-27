using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Speech.Recognition;
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

using Microsoft.CognitiveServices.Speech;

using static System.Net.Mime.MediaTypeNames;

namespace Jenny_V2.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly SpeechRecognizerService _speechRecognizerService;
        private readonly VoiceActivationService _voiceActivationService;

        public MainPage(
            SpeechRecognizerService speechRecognizerService,
            VoiceActivationService voiceActivationService,
            MainPageService mainPageService
            )
        {
            _speechRecognizerService = speechRecognizerService;
            _voiceActivationService = voiceActivationService;

            _voiceActivationService.VoiceActivationStart();

            mainPageService.JennyLog += JennyLog;
            mainPageService.LogNormal += Log;
            mainPageService.UserLog += UserLog;
            mainPageService.UpdateLightOn += UpdateLight;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _speechRecognizerService.ToggleSpeechRegonition();
            _voiceActivationService.ToggleVoiceActivation();
        }

        public void Log(string text)
        {
            if (text == "") return;
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                LstLogs.Items.Insert(0, text);
            });
        }

        public void JennyLog(string text)
        {
            if (text == "") return;
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                LstJennyText.Items.Insert(0, text);
            });
        }

        public void UserLog(string text)
        {
            if (text == "") return;
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                LstSpokenText.Items.Insert(0, text);
            });
        }

        public void UpdateLight(bool on)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                BrushConverter bc = new BrushConverter();

                if (on) CircleIsListening.Fill = (Brush)bc.ConvertFrom("Green")!;
                else CircleIsListening.Fill = (Brush)bc.ConvertFrom("Red")!;
            });
        }
    }
}
