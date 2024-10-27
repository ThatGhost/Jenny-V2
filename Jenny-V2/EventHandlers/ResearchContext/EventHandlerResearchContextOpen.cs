using System.IO;

using Jenny_V2.Pages;
using Jenny_V2.Services;

using static Jenny_V2.Services.ChatGPTService;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.ResearchContextOpen)]
    public class EventHandlerResearchContextOpen : IEventHandler
    {
        private readonly ResearchContextService _researchContextService;
        private readonly ChatGPTService _chatGPTService;
        private readonly SpeechRecognizerService _speechRecognizerService;
        private readonly ZeroShotService _zeroShotService;
        private readonly TextToSpeechService _textToSpeechService;
        private readonly MainPageService _mainPageService;

        public EventHandlerResearchContextOpen(
            ResearchContextService researchContextService,
            ChatGPTService chatGPTService,
            SpeechRecognizerService speechRecognizerService,
            ZeroShotService zeroShotService,
            TextToSpeechService textToSpeechService,
            MainPageService mainPageService
            )
        {
            _researchContextService = researchContextService;
            _chatGPTService = chatGPTService;
            _speechRecognizerService = speechRecognizerService;
            _zeroShotService = zeroShotService;
            _textToSpeechService = textToSpeechService;
            _mainPageService = mainPageService;
        }

        public void Handle(string text)
        {
            List<string> researchContexts = _researchContextService.GetAllResearchContexts();
            string fuzzySearchProject = _zeroShotService.FuzzySearch(text, researchContexts);
            if (fuzzySearchProject != null)
            {
                _researchContextService.SetResearchContext(fuzzySearchProject);
            }
            else
            {
                AskUser();
            }
        }

        private void AskUser()
        {
            List<string> researchContexts = _researchContextService.GetAllResearchContexts();

            _chatGPTService.GetAiResponse("ask the user if they want to hear all the options, return only your question");

            _zeroShotService.AddPossibilities(new List<string> { "Yes", "No" });
            string awnser = _zeroShotService.Listen();

            if (awnser == "Yes")
            {
                ListOptions();
            }

            string toSpeakText = "Which one would you like to open?";
            _textToSpeechService.SpeakAsync(toSpeakText);
            _mainPageService.JennyLog(toSpeakText);

            _zeroShotService.AddPossibilities(researchContexts.Select(r => r.Replace("_", " ")).ToList());
            awnser = _zeroShotService.Listen().Replace(" ", "_");

            if (awnser != "")
                _researchContextService.SetResearchContext(awnser);
            else
            {
                toSpeakText = "Sorry i didnt get that. Can you try again from the beginning?";
                _textToSpeechService.SpeakAsync(toSpeakText);
                _mainPageService.JennyLog(toSpeakText);
            }
        }

        private void ListOptions()
        {
            List<string> research = _researchContextService.GetAllResearchContextFolders();

            int maxAmountOfResearchToSpeak = 7;
            string toSpeakText = @$"You currently have {research.Count()} research context's. namely ";
            foreach (var context in research)
            {
                string contextname = new DirectoryInfo(context + "\\").Name;
                contextname = contextname.Replace("_", " ");
                toSpeakText += $"{contextname}, ";
            }

            if (maxAmountOfResearchToSpeak < research.Count) toSpeakText += "and are some more I havent listed yet.";

            _textToSpeechService.Speak(toSpeakText);
            _mainPageService.JennyLog(toSpeakText);
        }
    }
}
