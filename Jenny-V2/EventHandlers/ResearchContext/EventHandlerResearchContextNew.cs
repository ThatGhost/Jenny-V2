using Jenny_V2.Services;

using static Google.Rpc.Context.AttributeContext.Types;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextNew)]
    public class EventHandlerResearchContextNew : IEventHandler
    {
        private readonly ResearchContextService _researchContextService;
        private readonly ChatGPTService _chatGPTService;
        private readonly SpeechRecognizerService _speechRecognizerService;
        private readonly TextToSpeechService _textToSpeechService;

        public EventHandlerResearchContextNew(
            ResearchContextService researchContextService,
            ChatGPTService chatGPTService,
            SpeechRecognizerService speechRecognizerService,
            TextToSpeechService textToSpeechService
            )
        {
            _researchContextService = researchContextService;
            _chatGPTService = chatGPTService;
            _speechRecognizerService = speechRecognizerService;
            _textToSpeechService = textToSpeechService;
        }

        public void Handle(string text)
        {
            _chatGPTService.onAIResponse += OnAiResponse;
            _chatGPTService.AutoSpeak = false;
            _chatGPTService.GetAiResponse($"in this scentence is there a name for a research? if yes respond only with the name, if not respond only with 'no'. \n-'{text}'");
        }

        private void OnAiResponse(string response)
        {
            _chatGPTService.onAIResponse -= OnAiResponse;
            _chatGPTService.AutoSpeak = true;

            if (response.ToLower().Replace(".","") == "no")
            {
                _chatGPTService.GetAiResponse("ask the user what the name should be of the research context, be friendly and consice, awnser only with the question");
                _speechRecognizerService.AutoAwnser = false;
                _speechRecognizerService.onSpeechRegognized += OnSpeechRegognised;
            }
            else
            {
                _researchContextService.CreateNewResearchContext(response.ToLower().Replace(" ","_").Replace(".", ""));
                _textToSpeechService.Speak($"a new research module has been created named {response}");
                MainWindow.onJenny($"a new research module has been created named {response}");
            }
        }

        private void OnSpeechRegognised(string text)
        {
            _speechRecognizerService.AutoAwnser = true;
            _speechRecognizerService.onSpeechRegognized -= OnSpeechRegognised;

            text = text.ToLower().Replace("it ","").Replace("name ","").Replace("call ","").Replace(".","");
            _researchContextService.CreateNewResearchContext(text.Replace(" ", "_"));

            _textToSpeechService.Speak($"a new research module has been created named {text}");
            MainWindow.onJenny($"a new research module has been created named {text}");
        }
    }
}
