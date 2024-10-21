using AudioSwitcher.AudioApi.CoreAudio;

namespace Jenny_V2.Services
{
    public class VolumeService
    {
        private readonly CoreAudioDevice defaultPlaybackDevice;

        public VolumeService()
        {
            defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
        }

        public void VolumeUp()
        {
            if (defaultPlaybackDevice.Volume < 95)
                defaultPlaybackDevice.Volume += 5;
        }

        public void VolumeDown()
        {
            if (defaultPlaybackDevice.Volume > 5)
                defaultPlaybackDevice.Volume -= 5;
        }

        public void SetVolume(int volume)
        {
            defaultPlaybackDevice.Volume = volume;
        }

        public double Volume { get { return defaultPlaybackDevice.Volume; } }
    }
}
