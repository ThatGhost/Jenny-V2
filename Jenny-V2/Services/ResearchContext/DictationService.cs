
using System.IO;
using Jenny_V2.Pages;

namespace Jenny_V2.Services.ResearchContext
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
        public string CleanedText = "";
        public string SummarizedText = "";

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
            IsDictating = true;

            _mainWindow.Navigate<DictationsPage>();
            _textToSpeechService.SpeakAsync("I am listening");

            _speechRecognizerService.StartSpeechRegonition();
            _speechRecognizerService.AutoAwnser = false;
            _speechRecognizerService.onSpeechRegognized += OnDictation;

            if (OnDictactionHeard != null)
            {
                DictationText = GetDictationText();
                CleanedText = GetCleanText();
                SummarizedText = GetSummerizedText();
                UpdateUI();
            }
        }

        public void Stop()
        {
            if (!IsDictating) return;
            IsDictating = false;

            _speechRecognizerService.AutoAwnser = true;
            _speechRecognizerService.onSpeechRegognized -= OnDictation;

            Save();
        }

        public void OnDictation(string text)
        {
            DictationText += $" {text}";
            UpdateUI();
        }

        public void AddDictiationKeywords()
        {
            keywords.Clear();
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "clean" }, TextCommand.ResearchContextDictateClean));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "cleaner" }, TextCommand.ResearchContextDictateClean));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "improve" }, TextCommand.ResearchContextDictateClean));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "summarize" }, TextCommand.ResearchContextDictateSummarize));
            keywords.Add(new KeyValuePair<string[], TextCommand>(new string[] { "shorten" }, TextCommand.ResearchContextDictateSummarize));

            foreach (var keyword in keywords) _keywordService.AddTextCommand(keyword);
        }

        public void RemoveDictiationKeywords()
        {
            foreach (var key in keywords) _keywordService.RemoveKeyWordsOnReference(key);
        }

        private void UpdateUI()
        {
            if (OnDictactionHeard != null)
                OnDictactionHeard($"#Dictated Text$${DictationText}$$#Cleaned Text$${CleanedText}$$#Summarized Text$${SummarizedText}\n\n");
        }

        #region AITransformations
        public void CleanupText()
        {
            _chatGPTService.AutoSpeak = false;
            _chatGPTService.onAIResponse += OnAiResponseCleaned;

            _chatGPTService.GetAIResponse(@$"This text is spoken text by the user. can you clean it up so it makes more sense? 
                                            only return the cleaned up text.
                                            text: '{DictationText}'");
        }

        private void OnAiResponseCleaned(string text)
        {
            CleanedText = text;
            UpdateUI();
            SaveCleaned();
        }

        public void SummerizeText()
        {
            if (CleanedText == "")
            {
                _textToSpeechService.SpeakAsync("Please clean up the text first");
                return;
            }

            _chatGPTService.AutoSpeak = false;
            _chatGPTService.onAIResponse += OnAiResponseSummerize;

            _chatGPTService.GetAIResponse(@$"Can you summerize this text up so it is short and digestable? 
                                            only return the cleaned up text.
                                            text: '{CleanedText}'");
        }

        private void OnAiResponseSummerize(string text)
        {
            SummarizedText = text;
            UpdateUI();
            SaveSummerized();
        }

        #endregion

        #region IO
        public void Save()
        {
            string filePath = Path.Combine(_researchContextService.GetCurrentResearchContextFolder(), "dictation.txt");
            _fileService.SaveFileContent(filePath, DictationText);
        }

        public void SaveCleaned()
        {
            string filePath = Path.Combine(_researchContextService.GetCurrentResearchContextFolder(), "cleanDictation.txt");
            _fileService.SaveFileContent(filePath, CleanedText);
        }

        public void SaveSummerized()
        {
            string filePath = Path.Combine(_researchContextService.GetCurrentResearchContextFolder(), "summerizedDictation.txt");
            _fileService.SaveFileContent(filePath, SummarizedText);
        }

        private string GetDictationText()
        {
            string filePath = Path.Combine(_researchContextService.GetCurrentResearchContextFolder(), "dictation.txt");
            return _fileService.GetFileContent(filePath);
        }

        public string GetCleanText()
        {
            string filePath = Path.Combine(_researchContextService.GetCurrentResearchContextFolder(), "cleanDictation.txt");
            return _fileService.GetFileContent(filePath);
        }

        private string GetSummerizedText()
        {
            string filePath = Path.Combine(_researchContextService.GetCurrentResearchContextFolder(), "summerizedDictation.txt");
            return _fileService.GetFileContent(filePath);
        }

        #endregion
    }
}
