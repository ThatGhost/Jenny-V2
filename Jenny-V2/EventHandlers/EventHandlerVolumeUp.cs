using Jenny_V2.Services;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(Services.TextCommand.TurnVolumeUp)]
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
            MainWindow.onLog("volume: " + _volumeService.Volume);
        }
    }
}
