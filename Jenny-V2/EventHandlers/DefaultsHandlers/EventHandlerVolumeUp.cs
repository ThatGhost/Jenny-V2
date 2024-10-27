using Jenny_V2.Pages;
using Jenny_V2.Services;

namespace Jenny_V2.EventHandlers.DefaultsHandlers
{
    [EventHandler(TextCommand.TurnVolumeUp)]
    public class EventHandlerVolumeUp : IEventHandler
    {
        private readonly VolumeService _volumeService;
        public EventHandlerVolumeUp(VolumeService volumeService)
        {
            _volumeService = volumeService;
        }

        public void Handle(string text)
        {
            _volumeService.VolumeUp();
            MainPage.onLog("volume: " + _volumeService.Volume);
        }
    }
}
