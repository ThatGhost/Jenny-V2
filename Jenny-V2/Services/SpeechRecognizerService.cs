using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;

using Microsoft.Extensions.Configuration;

namespace Jenny_V2.Services
{
    public class SpeechRecognizerService
    {
        private IConfiguration Configuration { get; }
        private SpeechRecognizer speechRecognizer;
        public bool IsRegonizing { get; private set; }
        public delegate void SpeechRecognizerEventHandler(string text);
        public SpeechRecognizerEventHandler SpeechRecognized;

        public SpeechRecognizerService()
        {
            IsRegonizing = false;
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())  // Set the base path to the current directory
            .AddUserSecrets<App>();                        // Load user secrets based on your UserSecretsId

            Configuration = builder.Build();

            var AzureSpeechKey = Configuration["Speech:Key"];
            var AzureSpeechRegion = Configuration["Speech:Region"];

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
            speechRecognizer.StartContinuousRecognitionAsync();
            speechRecognizer.Recognized += OnSpeechRegognized;
            IsRegonizing = true;
        }

        public void StopSpeechRegonition()
        {
            speechRecognizer.StopContinuousRecognitionAsync();
            speechRecognizer.Recognized -= OnSpeechRegognized;
            IsRegonizing = false;
        }

        public void Dispose() => speechRecognizer?.Dispose();

        private void OnSpeechRegognized(object sender, SpeechRecognitionEventArgs args)
        {
            var speechRecognitionResult = args.Result;
            switch (speechRecognitionResult.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    if (SpeechRecognized != null && SpeechRecognized.GetInvocationList().Length > 0) SpeechRecognized.Invoke(speechRecognitionResult.Text);
                    break;
                case ResultReason.NoMatch:
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                    break;
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                    throw new Exception(cancellation.ErrorDetails);
            }
        }
    }
}
