using Jenny_V2.Services;

namespace Jenny_V2.EventHandlers
{
    [EventHandler(Services.TextCommand.GetVolume)]
    public class EventHandlerGetVolume : IEventHandler
    {
        private readonly VolumeService _volumeService;

        public EventHandlerGetVolume(
            VolumeService volumeService
            )
        {
            _volumeService = volumeService;
        }

        public void Handle(string text)
        {
            MainWindow.onLog("Volume: "+_volumeService.Volume);
        }
    }
}
