using System.Text.RegularExpressions;
using Jenny_V2.Pages;
using Jenny_V2.Services;
using Jenny_V2.Services.UI;

namespace Jenny_V2.EventHandlers.DefaultsHandlers
{
    [EventHandler(TextCommand.GetVolume)]
    public class EventHandlerGetVolume : IEventHandler
    {
        private readonly VolumeService _volumeService;
        private readonly TextToSpeechService _textToSpeechService;
        private readonly MainPageService _mainPageService;

        public EventHandlerGetVolume(
            VolumeService volumeService,
            TextToSpeechService textToSpeechService,
            MainPageService mainPageService
            )
        {
            _volumeService = volumeService;
            _textToSpeechService = textToSpeechService;
            _mainPageService = mainPageService;
        }

        public void Handle(string text)
        {
            double volume = _volumeService.Volume;
            _mainPageService.Log("Volume: " + volume);
            _textToSpeechService.SpeakAsync($"The volume is {volume}");
        }
    }
}
