using System.Text.RegularExpressions;
using Jenny_V2.Services;

namespace Jenny_V2.EventHandlers.DefaultsHandlers
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
            _chatGPTService.GetAIResponse("explain to the user that you can controll the volume, playing of media and help with research, keep it consice and friendly");
        }
    }
}
