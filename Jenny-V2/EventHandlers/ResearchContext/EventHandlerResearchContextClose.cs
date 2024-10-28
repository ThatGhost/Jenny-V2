using Jenny_V2.Pages;
using Jenny_V2.Services;
using Jenny_V2.Services.ResearchContext;
using WindowsInput;
using WindowsInput.Native;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextClose)]
    public class EventHandlerResearchContextClose : IEventHandler
    {
        private readonly ResearchContextService _researchContextService;
        private readonly MainWindow _mainWindow;
        private readonly DictationService _dictationService;

        public EventHandlerResearchContextClose(
            ResearchContextService researchContextService,
            MainWindow mainWindow,
            DictationService dictationService
            )
        {
            _researchContextService = researchContextService;
            _mainWindow = mainWindow;
            _dictationService = dictationService;
        }

        public void Handle(string text)
        {
            _researchContextService.CloseResearchContext();
            _mainWindow.Navigate<MainPage>();
            _dictationService.Stop();
        }
    }
}
