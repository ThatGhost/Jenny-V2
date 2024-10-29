using Jenny_V2.Services.Core;
using Jenny_V2.Services.ResearchContext;
using WindowsInput;
using WindowsInput.Native;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextDictateClear)]
    public class EventHandlerResearchContextDictateClear : IEventHandler
    {
        private readonly DictationService _dictationService;
        private readonly ZeroShotService _zeroShotService;
        private readonly TextToSpeechService _textToSpeechService;

        public EventHandlerResearchContextDictateClear(
            DictationService dictationService,
            ZeroShotService zeroShotService,
            TextToSpeechService textToSpeechService
            )
        {
            _dictationService = dictationService;
            _zeroShotService = zeroShotService;
            _textToSpeechService = textToSpeechService;
        }

        public void Handle(string text)
        {
            _textToSpeechService.Speak("Are you sure you want to delete the dictated text?");
            _zeroShotService.AddPossibilities(new List<string>() { "yes", "no" });
            string response = _zeroShotService.Listen();

            if(response == "yes") _dictationService.ClearDictatedText();
        }
    }
}
