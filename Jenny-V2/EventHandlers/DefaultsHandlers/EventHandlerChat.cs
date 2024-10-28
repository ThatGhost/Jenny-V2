using Jenny_V2.Pages;
using Jenny_V2.Services;

namespace Jenny_V2.EventHandlers.DefaultsHandlers
{
    [EventHandler(TextCommand.Chat)]
    public class EventHandlerChat : IEventHandler
    {
        private readonly MainWindow _mainWindow;
        public EventHandlerChat(
            MainWindow mainWindow
            )
        {
            _mainWindow = mainWindow;
        }

        public void Handle(string text)
        {
            _mainWindow.Navigate<ResearchContextChat>();
        }
    }
}
