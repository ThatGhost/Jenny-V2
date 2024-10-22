using Jenny_V2.EventHandlers.Core;
using Jenny_V2.EventHandlers;
using Jenny_V2.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Jenny_V2
{
    public static class Installer
    {
        public static void Install(IServiceCollection services)
        {
            services.AddSingleton<SpeechRecognizerService>();
            services.AddSingleton<ChatGPTService>();
            services.AddSingleton<TextToSpeechService>();
            services.AddTransient<EventFactory>();

            services.AddTransient<KeywordService>();
            services.AddTransient<VolumeService>();
            services.AddTransient<BrowserService>();
            services.AddTransient<ResearchContextService>();

            services.AddTransient<EventHandlerVolumeUp>();
            services.AddTransient<EventHandlerVolumeDown>();
            services.AddTransient<EventHandlerSetVolume>();
            services.AddTransient<EventHandlerGetVolume>();
            services.AddTransient<EventHandlerGetInfo>();
            services.AddTransient<EventHandlerPauseMedia>();
            services.AddTransient<EventHandlerShutdown>();
            services.AddTransient<EventHandlerResearchContextNew>();
            services.AddTransient<EventHandlerResearchContextOpen>();
        }
    }
}
