using System.Text.RegularExpressions;
using Jenny_V2.Pages;
using Jenny_V2.Services;
using Jenny_V2.Services.UI;

namespace Jenny_V2.EventHandlers.DefaultsHandlers
{
    [EventHandler(TextCommand.SetVolume)]
    public class EventHandlerSetVolume : IEventHandler
    {

        private readonly VolumeService _volumeService;
        private readonly ChatGPTService _chatGPTService;
        private readonly SpeechRecognizerService _speechRecognizerService;
        private readonly MainPageService _mainPageService;

        public EventHandlerSetVolume(
            VolumeService volumeService,
            ChatGPTService chatGPTService,
            SpeechRecognizerService speechRecognizerService,
            MainPageService mainPageService
            )
        {
            _volumeService = volumeService;
            _chatGPTService = chatGPTService;
            _speechRecognizerService = speechRecognizerService;
            _mainPageService = mainPageService;
        }

        public void Handle(string text)
        {
            string number = Regex.Match(text, @"\d+").Value;
            if (number == "")
            {
                _speechRecognizerService.AutoAwnser = false;
                _chatGPTService.onAIResponse += OnAiReponse;
                _chatGPTService.GetAIResponse("Ask the user what the volume should be set to");
            }
            else SetVolume(int.Parse(number));
        }

        private void SetVolume(int volume)
        {
            _volumeService.SetVolume(volume);
            _mainPageService.Log("volume: " + _volumeService.Volume);
        }

        private void OnSpeechRegonised(string text)
        {
            if (text.Trim() == "") return;

            _speechRecognizerService.onSpeechRegognized -= OnSpeechRegonised;
            string number = Regex.Match(text, @"\d+").Value;
            if (number == "") _mainPageService.JennyLog("Sorry i didnt catch that try it again");
            else SetVolume(int.Parse(number));

            _speechRecognizerService.AutoAwnser = true;
        }

        private void OnAiReponse(string text)
        {
            _chatGPTService.onAIResponse -= OnAiReponse;
            _speechRecognizerService.onSpeechRegognized += OnSpeechRegonised;
        }
    }
}
