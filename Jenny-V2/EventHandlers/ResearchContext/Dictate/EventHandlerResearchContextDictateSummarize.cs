using Jenny_V2.Services;

using WindowsInput;
using WindowsInput.Native;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextDictateSummarize)]
    public class EventHandlerResearchContextDictateSummarize : IEventHandler
    {
        private readonly DictationService _dictationService;

        public EventHandlerResearchContextDictateSummarize(
            DictationService dictationService
            )
        {
            _dictationService = dictationService;
        }

        public void Handle(string text)
        {
            _dictationService.SummerizeText();
        }
    }
}
