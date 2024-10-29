using System.IO;
using Jenny_V2.Services.Core;
using Jenny_V2.Services.ResearchContext;
using Jenny_V2.Services.UI;

using OpenAI.Chat;

namespace Jenny_V2.Services
{
    public class ChatService
    {
        private readonly DictationService _dictationService;
        private readonly ChatGPTService _chatGPTService;
        private readonly FileService _fileService;
        private readonly ResearchContextService _researchContextService;
        private readonly ChatPageService _chatPageService;
        private List<ChatMessage> _chatMessages = new List<ChatMessage>();
        private readonly string _chatPath;


        private const string _separationString = "$$--$$";

        public ChatService(
            DictationService dictationService,
            ChatGPTService chatGPTService,
            FileService fileService,
            ResearchContextService researchContextService,
            ChatPageService chatPageService
            )
        {
            _dictationService = dictationService;
            _chatGPTService = chatGPTService;
            _fileService = fileService;
            _researchContextService = researchContextService;
            _chatPageService = chatPageService;

            _chatPath = Path.Combine(_researchContextService.GetCurrentResearchContextFolder(), "chat.txt");
        }

        public void Start()
        {
            _chatGPTService.AutoSpeak = false;
            _chatGPTService.onAIResponse += OnAiResponse;
            _chatPageService.OnUpdateLabel(_researchContextService.GetResearchContext() ?? "General Chat");
            InitializeChatMessages();
        }

        public void Stop()
        {
            _chatGPTService.AutoSpeak = true;
            _chatGPTService.onAIResponse -= OnAiResponse;
        }

        private void OnAiResponse(string awnser)
        {
            _chatMessages.Add(new AssistantChatMessage(awnser));
            _chatPageService.OnMessageReceived(awnser);
            SaveChatMessages();
        }

        public void SendChat(string chat)
        {
            _chatMessages.Add(new UserChatMessage(chat));
            _chatGPTService.GetAIResponse(_chatMessages.ToArray());
        }

        private void SaveChatMessages()
        {
            string chatString = "";

            foreach (var message in _chatMessages)
            {
                chatString += $"{_separationString}{message.Content[0].Text}";
            }

            _fileService.SaveFileContent(_chatPath, chatString);
        }

        public void InitializeChatMessages()
        {
            string chatTextRaw = _fileService.GetFileContent(_chatPath);
            if (chatTextRaw == "") return;

            string[] chats = chatTextRaw.Split(_separationString);
            chats = chats.Take(chats.Length).ToArray();

            if(_researchContextService.IsInResearchContext())
            {
                string cleanedDictation = _dictationService.GetCleanText();
                _chatMessages.Add(new UserChatMessage($"the following text is context that you might want to consider.\n\n{cleanedDictation}"));
            }

            for (int i = 1; i < chats.Length; i++)
            {
                string chat = chats[i];
                if(i % 2 == 1)
                {
                    _chatMessages.Add(new UserChatMessage(chat));
                    _chatPageService.OnShowUserMessage(chat);
                }
                else
                {
                    _chatMessages.Add(new AssistantChatMessage(chat));
                    _chatPageService.OnMessageReceived(chat);
                }
            }
        }
    }
}
