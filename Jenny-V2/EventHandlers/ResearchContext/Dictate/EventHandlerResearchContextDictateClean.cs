using Jenny_V2.Services;
using Jenny_V2.Services.ResearchContext;
using WindowsInput;
using WindowsInput.Native;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextDictateClean)]
    public class EventHandlerResearchContextDictateClean : IEventHandler
    {
        private readonly DictationService _dictationService;

        public EventHandlerResearchContextDictateClean(
            DictationService dictationService
            )
        {
            _dictationService = dictationService;
        }

        public void Handle(string text)
        {
            _dictationService.CleanupText();
        }
    }
}
