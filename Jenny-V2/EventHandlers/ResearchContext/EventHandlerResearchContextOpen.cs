using Jenny_V2.Services;

using WindowsInput;
using WindowsInput.Native;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextOpen)]
    public class EventHandlerResearchContextOpen : IEventHandler
    {
        private readonly ResearchContextService _researchContextService;
        private readonly ChatGPTService _chatGPTService;
        private readonly SpeechRecognizerService _speechRecognizerService;

        public EventHandlerResearchContextOpen(
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
