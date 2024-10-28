using Jenny_V2.Services;
using Jenny_V2.Services.ResearchContext;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextDictate)]
    public class EventHandlerResearchContextDictate : IEventHandler
    {
        private readonly DictationService _dictationService;
        public EventHandlerResearchContextDictate(
            DictationService dictationService
            )
        {
            _dictationService = dictationService;
        }

        public void Handle(string text)
        {
            _dictationService.Start();
        }
    }
}
