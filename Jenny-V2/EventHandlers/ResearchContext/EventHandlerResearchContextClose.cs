using Jenny_V2.Services;

using WindowsInput;
using WindowsInput.Native;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextClose)]
    public class EventHandlerResearchContextClose : IEventHandler
    {
        private readonly ResearchContextService _researchContextService;

        public EventHandlerResearchContextClose(
            ResearchContextService researchContextService
            )
        {
            _researchContextService = researchContextService;
        }

        public void Handle(string text)
        {
            _researchContextService.CloseResearchContext();
        }
    }
}
