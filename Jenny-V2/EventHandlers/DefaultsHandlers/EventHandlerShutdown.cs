using System.Text.RegularExpressions;
using Jenny_V2.Services;

namespace Jenny_V2.EventHandlers.DefaultsHandlers
{
    [EventHandler(TextCommand.Shutdown)]
    public class EventHandlerShutdown : IEventHandler
    {
        public readonly SpeechRecognizerService _speechRecognizerService;

        public EventHandlerShutdown(
            SpeechRecognizerService speechRecognizerService
            )
        {
            _speechRecognizerService = speechRecognizerService;
        }

        public void Handle(string text)
        {
            _speechRecognizerService.StopSpeechRegonition();
        }
    }
}
