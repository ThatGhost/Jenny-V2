using System.Text.RegularExpressions;
using Jenny_V2.Services;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.GetInfo)]
    public class EventHandlerGetInfo : IEventHandler
    {
        private readonly TextToSpeechService _textToSpeechService;
        private readonly ChatGPTService _chatGPTService;

        public EventHandlerGetInfo(
            TextToSpeechService textToSpeechService,
            ChatGPTService chatGPTService
            )
        {
            _textToSpeechService = textToSpeechService;
            _chatGPTService = chatGPTService;
        }

        public void Handle(string text)
        {
            MainWindow.onJenny("Volume control");
            _chatGPTService.GetAiResponse("explain to the user that you can controll the volume and that your creator is working on the ability to pause and unpause music, keep it consice and friendly");
        }
    }
}
