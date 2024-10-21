using System.Text.RegularExpressions;
using Jenny_V2.Services;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.SetVolume)]
    public class EventHandlerSetVolume : IEventHandler
    {

        private readonly VolumeService _volumeService;
        private readonly ChatGPTService _chatGPTService;
        private readonly SpeechRecognizerService _speechRecognizerService;

        public EventHandlerSetVolume(
            VolumeService volumeService,
            ChatGPTService chatGPTService,
            SpeechRecognizerService speechRecognizerService
            )
        {
            _volumeService = volumeService;
            _chatGPTService = chatGPTService;
            _speechRecognizerService = speechRecognizerService;
        }

        public void Handle(string text)
        {
            string number = Regex.Match(text, @"\d+").Value;
            if (number == "")
            {
                MainWindow.AutoAwnser = false;
                _chatGPTService.onAIResponse += OnAiReponse;
                _chatGPTService.GetAiResponse("Ask the user what the volume should be set to");
            }
            else SetVolume(int.Parse(number));

        }

        private void SetVolume(int volume)
        {
            _volumeService.SetVolume(volume);
            MainWindow.onLog("volume: " + _volumeService.Volume);
        }

        private void OnSpeechRegonised(string text)
        {
            _speechRecognizerService.SpeechRecognized -= OnSpeechRegonised;
            string number = Regex.Match(text, @"\d+").Value;
            if (number == "") MainWindow.onJenny("Sorry i didnt catch that try it again");
            else SetVolume(int.Parse(number));

            MainWindow.AutoAwnser = true;
        }

        private void OnAiReponse(string text)
        {
            _chatGPTService.onAIResponse -= OnAiReponse;
            _speechRecognizerService.SpeechRecognized += OnSpeechRegonised;
        }
    }
}
