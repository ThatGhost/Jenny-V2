using System.Configuration;
using System.IO;

using Google.Cloud.TextToSpeech.V1;

using Microsoft.Extensions.Configuration;

using NAudio.Wave;

namespace Jenny_V2.Services
{
    public class TextToSpeechService
    {
        private TextToSpeechClient textToSpeechClient;
        private VoiceSelectionParams voiceSelectionParams;
        private readonly AudioConfig audioConfig;

        public TextToSpeechService()
        {
            audioConfig = new AudioConfig { AudioEncoding = AudioEncoding.Linear16 };
            voiceSelectionParams = new VoiceSelectionParams
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
            textToSpeechClient = builder.Build();
        }

        public void Speak(string text)
        {
            var input = new SynthesisInput { Text = text };
            var response = textToSpeechClient.SynthesizeSpeech(input, voiceSelectionParams, audioConfig);

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

    }
}
