using System.Speech.Recognition;
using FuzzySharp;

namespace Jenny_V2.Services.Core
{
    public class ZeroShotService
    {
        private SpeechRecognitionEngine _speechRecognitionEngine;
        private List<string> _possibleCommands = new();
        public delegate void onSpeechRegonised(SpeechRecognizedEventArgs args);
        public onSpeechRegonised OnSpeechRegonised;

        public ZeroShotService()
        {
            _speechRecognitionEngine = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            _possibleCommands.Add("---");

            _speechRecognitionEngine.SetInputToDefaultAudioDevice();
            _speechRecognitionEngine.LoadGrammar(BuildGrammar());
        }

        ~ZeroShotService()
        {
            _speechRecognitionEngine.RecognizeAsyncStop();
            _speechRecognitionEngine?.Dispose();
        }

        public string Listen() => _speechRecognitionEngine.Recognize().Text;

        public void ListenAsyncStart()
        {
            _speechRecognitionEngine.RecognizeAsync();
            _speechRecognitionEngine.SpeechRecognized += OnSpeechRecognizedAsync;
        }

        public void ListenAsyncStop()
        {
            _speechRecognitionEngine.RecognizeAsyncStop();
            _speechRecognitionEngine.SpeechRecognized -= OnSpeechRecognizedAsync;
        }

        private void OnSpeechRecognizedAsync(object? sender, SpeechRecognizedEventArgs args)
        {
            OnSpeechRegonised?.Invoke(args);
        }

        public void AddPossibilities(List<string> values)
        {
            _possibleCommands = values;
            _speechRecognitionEngine.UnloadAllGrammars();
            _speechRecognitionEngine.LoadGrammar(BuildGrammar());
        }

        public string FuzzySearch(string spokenSentence, List<string> projectNames)
        {
            // Iterate through project names and find the best match
            var bestMatch = projectNames
                .Select(project => new
                {
                    Name = project,
                    Score = Fuzz.PartialRatio(spokenSentence, project)
                })
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            // If the best match score is above a threshold, consider it a match
            if (bestMatch != null && bestMatch.Score >= 60) // Adjust threshold as needed
            {
                return bestMatch.Name;
            }

            return null; // No close match found
        }

        private Grammar BuildGrammar()
        {
            Choices commands = new();
            commands.Add(_possibleCommands.ToArray());

            // builder
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(commands);

            // build
            _possibleCommands.Clear();
            return new Grammar(grammarBuilder);
        }
    }
}
