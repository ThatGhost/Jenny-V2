using System.IO;

using Jenny_V2.Pages;
using Jenny_V2.Services;

using static Google.Rpc.Context.AttributeContext.Types;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextDictate)]
    public class EventHandlerResearchContextDictate : IEventHandler
    {
        private readonly ResearchContextService _researchContextService;
        private readonly TextToSpeechService _textToSpeechService;

        public EventHandlerResearchContextDictate(
            ResearchContextService researchContextService,
            TextToSpeechService textToSpeechService
            )
        {
            _researchContextService = researchContextService;
            _textToSpeechService = textToSpeechService;
        }

        public void Handle(string text)
        {
            string toSpeakText = "I am listening";
            _textToSpeechService.SpeakAsync(toSpeakText);
            MainPage.onJenny(toSpeakText);
        }
    }
}
