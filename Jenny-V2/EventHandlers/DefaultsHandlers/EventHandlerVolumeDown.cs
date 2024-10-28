using Jenny_V2.Pages;
using Jenny_V2.Services;
using Jenny_V2.Services.Core;
using Jenny_V2.Services.UI;

namespace Jenny_V2.EventHandlers.DefaultsHandlers
{
    [EventHandler(TextCommand.TurnVolumeDown)]
    public class EventHandlerVolumeDown : IEventHandler
    {
        private readonly VolumeService _volumeService;
        private readonly MainPageService _mainPageService;

        public EventHandlerVolumeDown(VolumeService volumeService, MainPageService mainPageService)
        {
            _volumeService = volumeService;
            _mainPageService = mainPageService;
        }

        public void Handle(string text)
        {
            _volumeService.VolumeDown();
            _mainPageService.Log("volume: " + _volumeService.Volume);
        }
    }
}
