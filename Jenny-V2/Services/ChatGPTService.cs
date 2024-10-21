using System.IO;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

namespace Jenny_V2.Services
{
    public class ChatGPTService
    {
        private IConfiguration Configuration { get; }
        public delegate void OnAIResponse(string response);
        public OnAIResponse onAIResponse;
        private OpenAIClient client;

        public ChatGPTService()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())  // Set the base path to the current directory
            .AddUserSecrets<App>();                        // Load user secrets based on your UserSecretsId

            Configuration = builder.Build();

            string openAIKey = Configuration["OpenAI:Key"];
            client = new OpenAIClient(openAIKey);
        }

        public async void GetAiResponse(string prompt)
        {
            try
            {
                var model = client.GetChatClient("gpt-4o-mini");
                ChatCompletion response = model.CompleteChat(new ChatMessage[]
                {
                    new UserChatMessage(prompt)
                });

                // Return the response content
                if (onAIResponse != null) onAIResponse.Invoke(response.Content.First().Text);
            }
            catch (Exception ex)
            {
                MainWindow.onLog("Something Went wrong" + ex.Message);
            }
        }
    }
}
