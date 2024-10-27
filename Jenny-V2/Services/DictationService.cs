
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

        public delegate void DictationScentence(string text);
        public DictationScentence? OnDictactionHeard;
        public bool IsDictating = false;
        public string DictationText = "";

        public DictationService(
            ResearchContextService researchContextService,
            TextToSpeechService textToSpeechService,
            MainWindow mainWindow,
            SpeechRecognizerService speechRecognizerService,
            FileService fileService
            )
        {
            _researchContextService = researchContextService;
            _textToSpeechService = textToSpeechService;
            _mainWindow = mainWindow;
            _speechRecognizerService = speechRecognizerService;
            _fileService = fileService;
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
            if (OnDictactionHeard != null) OnDictactionHeard(GetDictationText());
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
    }
}
