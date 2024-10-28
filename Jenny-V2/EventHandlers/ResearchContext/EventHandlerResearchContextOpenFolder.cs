using Jenny_V2.Services;
using Jenny_V2.Services.ResearchContext;
using WindowsInput;
using WindowsInput.Native;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextOpenFolder)]
    public class EventHandlerResearchContextOpenFolder : IEventHandler
    {
        private readonly ResearchContextService _researchContextService;

        public EventHandlerResearchContextOpenFolder(
            ResearchContextService researchContextService
            )
        {
            _researchContextService = researchContextService;
        }

        public void Handle(string text)
        {
            _researchContextService.OpenResearchContextFolder();
        }
    }
}
