using Jenny_V2.Services.Core;
using WindowsInput;
using WindowsInput.Native;

namespace Jenny_V2.EventHandlers.DefaultsHandlers
{
    [EventHandler(TextCommand.PlayPauseMedia)]
    public class EventHandlerPauseMedia : IEventHandler
    {
        private InputSimulator _simulator;
        public EventHandlerPauseMedia()
        {
            _simulator = new InputSimulator();
        }

        public void Handle(string text)
        {
            _simulator.Keyboard.KeyPress(VirtualKeyCode.MEDIA_PLAY_PAUSE);
        }
    }
}
