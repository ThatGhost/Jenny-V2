using Jenny_V2.Services;

using WindowsInput;
using WindowsInput.Native;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextNew)]
    public class EventHandlerResearchContextNew : IEventHandler
    {
        private readonly ResearchContextService _researchContextService;
        private readonly ChatGPTService _chatGPTService;
        private readonly SpeechRecognizerService _speechRecognizerService;

        public EventHandlerResearchContextNew(
            ResearchContextService researchContextService,
            ChatGPTService chatGPTService,
            SpeechRecognizerService speechRecognizerService
            )
        {
            _researchContextService = researchContextService;
            _chatGPTService = chatGPTService;
            _speechRecognizerService = speechRecognizerService;
        }

        public void Handle(string text)
        {

        }
    }
}
