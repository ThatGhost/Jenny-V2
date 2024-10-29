using System.IO;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;

using Microsoft.Extensions.Configuration;
using Jenny_V2.EventHandlers.Core;
using Jenny_V2.Pages;
using Jenny_V2.Services.UI;

namespace Jenny_V2.Services.Core
{
    public class SpeechRecognizerService
    {
        private readonly ChatGPTService _chatGPTService;
        private readonly KeywordService _keywordService;
        private readonly EventFactory _eventFactory;
        private readonly MainPageService _mainPageService;

        private SpeechRecognizer speechRecognizer;
        public bool IsRegonizing { get; private set; }
        public bool EnableSpeechRegognition = true;

        public delegate void OnSpeechRegognizedEvent(string text);
        public OnSpeechRegognizedEvent onSpeechRegognized;
        public static Action<bool> EnableSpeechRegognitionAction;

        public SpeechRecognizerService(
                KeywordService keywordService,
                EventFactory eventFactory,
                MainPageService mainPageService,
                ChatGPTService chatGPTService
            )
        {
            _chatGPTService = chatGPTService;
            _keywordService = keywordService;
            _eventFactory = eventFactory;
            _mainPageService = mainPageService;

            onSpeechRegognized += SpeechRegognized;
            EnableSpeechRegognitionAction += (bool enable) => { EnableSpeechRegognition = enable; };
            InitializeSpeechRecognizer();
        }

        ~SpeechRecognizerService()
        {
            if (IsRegonizing)
            {
                Task.Run(async () => await speechRecognizer.StopContinuousRecognitionAsync());
            }
            speechRecognizer?.Dispose();
        }

        private void InitializeSpeechRecognizer()
        {
            IsRegonizing = false;
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())  // Set the base path to the current directory
            .AddUserSecrets<App>();                        // Load user secrets based on your UserSecretsId

            IConfiguration configuration = builder.Build();

            var AzureSpeechKey = configuration["Speech:Key"];
            var AzureSpeechRegion = configuration["Speech:Region"];

            var speechConfig = SpeechConfig.FromSubscription(AzureSpeechKey, AzureSpeechRegion);
            speechConfig.SpeechRecognitionLanguage = "en-US";

            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);
        }

        public void ToggleSpeechRegonition()
        {
            if (IsRegonizing) StopSpeechRegonition();
            else StartSpeechRegonition();
        }

        public void StartSpeechRegonition()
        {
            if (IsRegonizing) return;

            Task.Run(async () => await speechRecognizer.StartContinuousRecognitionAsync());
            speechRecognizer.Recognized += OnSpeechRegognized;
            IsRegonizing = true;
            _mainPageService.UpdateLight(IsRegonizing);
        }

        public void StopSpeechRegonition()
        {
            if (!IsRegonizing) return;

            Task.Run(async () => await speechRecognizer.StopContinuousRecognitionAsync());
            speechRecognizer.Recognized -= OnSpeechRegognized;
            IsRegonizing = false;
            _mainPageService.UpdateLight(IsRegonizing);
        }

        private void OnSpeechRegognized(object sender, SpeechRecognitionEventArgs args)
        {
            var speechRecognitionResult = args.Result;
            switch (speechRecognitionResult.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    onSpeechRegognized.Invoke(speechRecognitionResult.Text);
                    break;
                case ResultReason.NoMatch:
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                    break;
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                    throw new Exception(cancellation.ErrorDetails);
            }
        }

        private void SpeechRegognized(string text)
        {
            if (text.Trim() == "" || !EnableSpeechRegognition) return;
            _mainPageService.UserLog(text);

            TextCommand? textCommand = _keywordService.FindTextCommand(text);

            if (textCommand != null)
            {
                _eventFactory.HandleEvent(textCommand.Value, text);
                _mainPageService.Log(textCommand.ToString() ?? "");
                return;
            }

            if (text.ToLower().Contains("jenny"))
            {
                _chatGPTService.GetAIResponse($"your name is jenny.\nCan you respond to the user in a exited and consise manner?\nuser- '{text}'");
            }
        }
    }
}
