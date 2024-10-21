using System.Text.RegularExpressions;
using Jenny_V2.Services;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.GetVolume)]
    public class EventHandlerGetVolume : IEventHandler
    {
        private readonly VolumeService _volumeService;
        private readonly TextToSpeechService _textToSpeechService;

        public EventHandlerGetVolume(
            VolumeService volumeService,
            TextToSpeechService textToSpeechService
            )
        {
            _volumeService = volumeService;
            _textToSpeechService = textToSpeechService;
        }

        public void Handle(string text)
        {
            double volume = _volumeService.Volume;
            MainWindow.onLog("Volume: " + volume);
            _textToSpeechService.Speak($"The volume is {volume}");
        }
    }
}
