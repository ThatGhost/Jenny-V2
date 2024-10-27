using System.Text.RegularExpressions;

using Jenny_V2.Pages;
using Jenny_V2.Services;

namespace Jenny_V2.EventHandlers.DefaultsHandlers
{
    [EventHandler(TextCommand.Shutdown)]
    public class EventHandlerShutdown : IEventHandler
    {
        private readonly SpeechRecognizerService _speechRecognizerService;
        private readonly MainWindow _mainWindow;
        private readonly VoiceActivationService _voiceActivationService;

        public EventHandlerShutdown(
            SpeechRecognizerService speechRecognizerService,
            MainWindow mainWindow,
            VoiceActivationService voiceActivationService
            )
        {
            _speechRecognizerService = speechRecognizerService;
            _mainWindow = mainWindow;
            _voiceActivationService = voiceActivationService;
        }

        public void Handle(string text)
        {
            _mainWindow.Navigate<MainPage>();
            _speechRecognizerService.StopSpeechRegonition();
            _voiceActivationService.VoiceActivationStart();
        }
    }
}
