
using System.IO;

using Jenny_V2.Pages;

namespace Jenny_V2.Services
{
    public class DictationService
    {
        private readonly ResearchContextService _researchContextService;
        private readonly TextToSpeechService _textToSpeechService;
        private readonly MainWindow _mainWindow;
        private readonly SpeechRecognizerService _speechRecognizerService;
        private readonly FileService _fileService;
        private readonly KeywordService _keywordService;
        private readonly ChatGPTService _chatGPTService;

        public delegate void DictationScentence(string text);
        public DictationScentence? OnDictactionHeard;
        public bool IsDictating = false;
        public string DictationText = "";

        private List<KeyValuePair<string[], TextCommand>> keywords = new List<KeyValuePair<string[], TextCommand>>();

        public DictationService(
            ResearchContextService researchContextService,
            TextToSpeechService textToSpeechService,
            MainWindow mainWindow,
            SpeechRecognizerService speechRecognizerService,
            FileService fileService,
            KeywordService keywordService,
            ChatGPTService chatGPTService
            )
        {
            _researchContextService = researchContextService;
            _textToSpeechService = textToSpeechService;
            _mainWindow = mainWindow;
            _speechRecognizerService = speechRecognizerService;
            _fileService = fileService;
            _keywordService = keywordService;
            _chatGPTService = chatGPTService;
        }

        public void Start()
        {
            if (IsDictating) return;

            _mainWindow.Navigate<DictationsPage>();
            _textToSpeechService.SpeakAsync("I am listening");

            _speechRecognizerService.StartSpeechRegonition();
            _speechRecognizerService.AutoAwnser = false;
            _speechRecognizerService.onSpeechRegognized += OnDictation;

            IsDictating = true;
            if (OnDictactionHeard != null)
            {
                DictationText = GetDictationText();
                OnDictactionHeard(DictationText);
            }
        }

        public void Stop()
        {
            if (!IsDictating) return;

            _speechRecognizerService.AutoAwnser = true;
            _speechRecognizerService.onSpeechRegognized -= OnDictation;

            IsDictating = false;
            Save();
        }

        public void Save()
        {
            string filePath = Path.Combine(_researchContextService.GetCurrentResearchContextFolder(), "dictation.txt");
            _fileService.SaveFileContent(filePath, DictationText);
        }

        private string GetDictationText()
        {
            string filePath = Path.Combine(_researchContextService.GetCurrentResearchContextFolder(), "dictation.txt");
            return _fileService.GetFileContent(filePath);
        }

        public void OnDictation(string text)
        {
            DictationText += $" {text}";
            if(OnDictactionHeard != null) OnDictactionHeard(DictationText);
        }

        public void AddDictiationKeywords()
        {
            keywords.Clear();
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "clean" }, TextCommand.ResearchContextDictateClean));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "summurize" }, TextCommand.ResearchContextDictateSummerize));
            
            foreach(var keyword in keywords) _keywordService.AddTextCommand(keyword);
        }

        public void RemoveDictiationKeywords()
        {
            foreach(var key in keywords) _keywordService.RemoveKeyWordsOnReference(key);
        }

        public void CleanupText()
        {
            _chatGPTService.AutoSpeak = false;
            _chatGPTService.onAIResponse += OnAiResponse;

            _chatGPTService.GetAiResponse(@$"This text is spoken text by the user. can you clean it up so it makes more sense? 
                                            only return the cleaned up text.
                                            text: '{DictationText}'");
        }

        private void OnAiResponse(string text)
        {
            DictationText = $"# Cleaned Text $${text}$$";
            if (OnDictactionHeard != null) OnDictactionHeard(DictationText);
            Save();
        }
    }
}
