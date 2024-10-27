using Jenny_V2.Pages;
using Jenny_V2.Services;

namespace Jenny_V2.EventHandlers.DefaultsHandlers
{
    [EventHandler(TextCommand.TurnVolumeUp)]
    public class EventHandlerVolumeUp : IEventHandler
    {
        private readonly VolumeService _volumeService;
        private readonly MainPageService _mainPageService;

        public EventHandlerVolumeUp(VolumeService volumeService, MainPageService mainPageService)
        {
            _volumeService = volumeService;
            _mainPageService = mainPageService;
        }

        public void Handle(string text)
        {
            _volumeService.VolumeUp();
            _mainPageService.Log("volume: " + _volumeService.Volume);
        }
    }
}
