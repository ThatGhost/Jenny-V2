using Jenny_V2.Pages;
using Jenny_V2.Services.Core;
using Jenny_V2.Services.ResearchContext;
using Jenny_V2.Services.UI;
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
        private readonly MainPageService _mainPageService;

        public EventHandlerResearchContextNew(
            ResearchContextService researchContextService,
            ChatGPTService chatGPTService,
            SpeechRecognizerService speechRecognizerService,
            TextToSpeechService textToSpeechService,
            MainPageService mainPageService
            )
        {
            _researchContextService = researchContextService;
            _chatGPTService = chatGPTService;
            _speechRecognizerService = speechRecognizerService;
            _textToSpeechService = textToSpeechService;
            _mainPageService = mainPageService;
        }

        public void Handle(string text)
        {
            _chatGPTService.onAIResponse += OnAiResponse;
            _chatGPTService.AutoSpeak = false;
            _chatGPTService.GetAIResponse($"in this scentence is there a name for a research in the next scentence? if yes respond only with the name, if not respond only with 'no'.\nscentence: '{text}'");
        }

        private void OnAiResponse(string response)
        {
            _chatGPTService.onAIResponse -= OnAiResponse;
            _chatGPTService.AutoSpeak = true;

            if (response.ToLower().Replace(".","") == "no")
            {
                _chatGPTService.GetAIResponse("ask the user what the name should be of the research context, be friendly and consice, awnser only with the question");
                _speechRecognizerService.AutoAwnser = false;
                _speechRecognizerService.onSpeechRegognized += OnSpeechRegognised;
            }
            else
            {
                _researchContextService.CreateNewResearchContext(response.ToLower().Replace(" ","_").Replace(".", ""));
                _textToSpeechService.SpeakAsync($"a new research module has been created named {response}");
                _mainPageService.JennyLog($"a new research module has been created named {response}");
            }
        }

        private void OnSpeechRegognised(string text)
        {
            _speechRecognizerService.AutoAwnser = true;
            _speechRecognizerService.onSpeechRegognized -= OnSpeechRegognised;

            text = text.ToLower().Replace("it ","").Replace("name ","").Replace("call ","").Replace(".","");
            _researchContextService.CreateNewResearchContext(text.Replace(" ", "_"));

            _textToSpeechService.SpeakAsync($"a new research module has been created named {text}");
            _mainPageService.JennyLog($"a new research module has been created named {text}");
        }
    }
}
