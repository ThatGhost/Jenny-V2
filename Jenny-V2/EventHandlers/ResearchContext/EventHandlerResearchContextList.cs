using System.IO;

using Jenny_V2.Services;

using static Google.Rpc.Context.AttributeContext.Types;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextList)]
    public class EventHandlerResearchContextList : IEventHandler
    {
        private readonly ResearchContextService _researchContextService;
        private readonly TextToSpeechService _textToSpeechService;

        public EventHandlerResearchContextList(
            ResearchContextService researchContextService,
            TextToSpeechService textToSpeechService
            )
        {
            _researchContextService = researchContextService;
            _textToSpeechService = textToSpeechService;
        }

        public void Handle(string text)
        {
            List<string> research = _researchContextService.GetAllResearchContexts();

            int maxAmountOfResearchToSpeak = 7;
            string toSpeakText = @$"You currently have {research.Count()} research context's. namely ";

            // parse
            foreach (var context in research) toSpeakText += $"{context.Replace("_", " ")}, ";

            if (maxAmountOfResearchToSpeak < research.Count) toSpeakText += ", and are some more I havent listed yet.";

            _textToSpeechService.SpeakAsync(toSpeakText);
            MainWindow.onJenny(toSpeakText);
        }
    }
}
