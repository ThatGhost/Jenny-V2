using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Azure.AI.OpenAI;

using Azure;

using Microsoft.Extensions.Configuration;

using OpenAI.Chat;

namespace Jenny_V2.Services
{
    public class ChatGPTService
    {
        private IConfiguration Configuration { get; }
        public delegate void OnAIResponse(string response);
        public OnAIResponse onAIResponse;
        private ChatClient _chatClient;

        public ChatGPTService()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())  // Set the base path to the current directory
            .AddUserSecrets<App>();                        // Load user secrets based on your UserSecretsId

            Configuration = builder.Build();

            string AzureOpenAiKey = Configuration["OpenAi:Key"]!;
            string AzureOpenAiUri = Configuration["OpenAi:URI"]!;

            AzureOpenAIClient azureClient = new(
            new Uri(AzureOpenAiUri),
            new System.ClientModel.ApiKeyCredential(AzureOpenAiKey));

            // This must match the custom deployment name you chose for your model
            _chatClient = azureClient.GetChatClient("gpt-4");
        }

        public async void GetAiResponse(string prompt)
        {
            try
            {
                ChatCompletion completion = await _chatClient.CompleteChatAsync([new UserChatMessage(prompt)]);

                if (onAIResponse != null && completion.FinishReason == ChatFinishReason.Stop) onAIResponse.Invoke(completion.Content[0].Text);

            }
            catch (Exception ex)
            {
                MainWindow.onLog("Rate Limited");
            }
        }
    }
}
