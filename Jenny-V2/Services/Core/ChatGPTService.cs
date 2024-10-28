using System.IO;
using System.Speech.Synthesis;
using Jenny_V2.Pages;
using Jenny_V2.Services.UI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

namespace Jenny_V2.Services.Core
{
    public class ChatGPTService
    {
        public delegate void OnAIResponse(string response);
        public OnAIResponse onAIResponse;
        public bool AutoSpeak = true;

        private OpenAIClient client;
        private readonly TextToSpeechService _textToSpeechService;
        private readonly MainPageService _mainPageService;


        public ChatGPTService(
            TextToSpeechService textToSpeechService,
            MainPageService mainPageService
            )
        {
            _textToSpeechService = textToSpeechService;
            _mainPageService = mainPageService;

            onAIResponse += SpeakOnAiResponse;

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())  // Set the base path to the current directory
            .AddUserSecrets<App>();                        // Load user secrets based on your UserSecretsId

            IConfiguration configuration = builder.Build();

            string openAIKey = configuration["OpenAI:Key"]!;
            client = new OpenAIClient(openAIKey);
        }

        public void GetAIResponse(string prompt)
        {
            GetAIResponse(new ChatMessage[] { new UserChatMessage(prompt) });
        }

        public void GetAIResponse(ChatMessage[] chatMessages)
        {
            Task.Run(() =>
            {
                try
                {
                    var model = client.GetChatClient("gpt-4o-mini");
                    ChatCompletion response = model.CompleteChat(chatMessages);

                    // Return the response content
                    if (onAIResponse != null) onAIResponse.Invoke(response.Content.First().Text);
                }
                catch (Exception ex)
                {
                    _mainPageService.Log("Something Went wrong" + ex.Message);
                }
            });
        }

        private void SpeakOnAiResponse(string text)
        {
            if (!AutoSpeak) return;
            _mainPageService.JennyLog(text);
            _textToSpeechService.SpeakAsync(text);
        }
    }
}
