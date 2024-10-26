using System.IO;
using System.Windows.Navigation;

using Jenny_V2.Pages;
using Jenny_V2.Services;

using static Google.Rpc.Context.AttributeContext.Types;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextDictate)]
    public class EventHandlerResearchContextDictate : IEventHandler
    {
        private readonly ResearchContextService _researchContextService;
        private readonly TextToSpeechService _textToSpeechService;
        private readonly MainWindow _mainWindow;
        private readonly SpeechRecognizerService _speechRecognizerService;

        public EventHandlerResearchContextDictate(
            ResearchContextService researchContextService,
            TextToSpeechService textToSpeechService,
            MainWindow mainWindow,
            SpeechRecognizerService speechRecognizerService
            )
        {
            _researchContextService = researchContextService;
            _textToSpeechService = textToSpeechService;
            _mainWindow = mainWindow;
            _speechRecognizerService = speechRecognizerService;
        }

        ~EventHandlerResearchContextDictate()
        {
            _speechRecognizerService.onSpeechRegognized -= OnDictation;
        }

        public void Handle(string text)
        {
            _mainWindow.Navigate<DictationsPage>();

            string toSpeakText = "I am listening";
            _textToSpeechService.SpeakAsync(toSpeakText);
            MainPage.onJenny(toSpeakText);

            _speechRecognizerService.AutoAwnser = false;
            _speechRecognizerService.onSpeechRegognized += OnDictation;
        }

        public void OnDictation(string text)
        {
            DictationsPage.addToList(text);
        }
    }
}
