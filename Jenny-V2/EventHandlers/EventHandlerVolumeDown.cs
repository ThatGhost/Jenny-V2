using Jenny_V2.Services;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(TextCommand.TurnVolumeDown)]
    public class EventHandlerVolumeDown : IEventHandler
    {
        private readonly VolumeService _volumeService;
        public EventHandlerVolumeDown(VolumeService volumeService)
        {
            _volumeService = volumeService;
        }

        public void Handle(string text)
        {
            _volumeService.VolumeDown();
            MainWindow.onLog("volume: " + _volumeService.Volume);
        }
    }
}
