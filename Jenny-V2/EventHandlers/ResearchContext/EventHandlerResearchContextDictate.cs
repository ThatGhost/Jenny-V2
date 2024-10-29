using Jenny_V2.Pages;
using Jenny_V2.Services.Core;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextDictate)]
    public class EventHandlerResearchContextDictate : IEventHandler
    {
        private readonly MainWindow _mainWindow;
        public EventHandlerResearchContextDictate(
            MainWindow mainWindow
            )
        {
            _mainWindow = mainWindow;
        }

        public void Handle(string text)
        {
            _mainWindow.Navigate<DictationsPage>();
        }
    }
}
