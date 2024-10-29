using System.Configuration;
using System.IO;

using Google.Cloud.TextToSpeech.V1;
using Microsoft.Extensions.Configuration;

using NAudio.Wave;

namespace Jenny_V2.Services.Core
{
    public class TextToSpeechService
    {
        private TextToSpeechClient _textToSpeechClient;
        private VoiceSelectionParams _voiceSelectionParams;
        private readonly AudioConfig _audioConfig;

        public TextToSpeechService()
        {
            _audioConfig = new AudioConfig { AudioEncoding = AudioEncoding.Linear16 };
            _voiceSelectionParams = new VoiceSelectionParams
            {
                LanguageCode = "en-US",
                SsmlGender = SsmlVoiceGender.Female,
            };
            TextToSpeechClientBuilder builder = new TextToSpeechClientBuilder();

            var ConfigurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<App>();

            IConfiguration configuration = ConfigurationBuilder.Build();

            builder.ApiKey = configuration["Google:Key"];
            _textToSpeechClient = builder.Build();
        }

        public void Speak(string text)
        {
            var input = new SynthesisInput { Text = text };
            var response = _textToSpeechClient.SynthesizeSpeech(input, _voiceSelectionParams, _audioConfig);

            // Use MemoryStream to play audio
            using (var ms = new MemoryStream(response.AudioContent.ToByteArray()))
            {
                using (var waveStream = new WaveFileReader(ms))
                {
                    using (var waveOut = new WaveOutEvent())
                    {
                        waveOut.Init(waveStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            Thread.Sleep(100); // Wait for the audio to finish playing
                        }
                    }
                }
            }
        }

        public void SpeakAsync(string text)
        {
            var input = new SynthesisInput { Text = text };
            var response = _textToSpeechClient.SynthesizeSpeech(input, _voiceSelectionParams, _audioConfig);

            // Use MemoryStream to play audio
            Task.Run(() =>
            {
                SpeechRecognizerService.EnableSpeechRegognitionAction(false);
                using (var ms = new MemoryStream(response.AudioContent.ToByteArray()))
                {
                    using (var waveStream = new WaveFileReader(ms))
                    {
                        using (var waveOut = new WaveOutEvent())
                        {
                            waveOut.Init(waveStream);
                            waveOut.Play();
                            while (waveOut.PlaybackState == PlaybackState.Playing)
                            {
                                Thread.Sleep(100); // Wait for the audio to finish playing
                            }
                        }
                    }
                }
                Thread.Sleep(1000); // whait 1s before turning it back on
                SpeechRecognizerService.EnableSpeechRegognitionAction(true);
            });
        }

    }
}
