using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jenny_V2.EventHandlers.Core;
using Jenny_V2.EventHandlers;
using Jenny_V2.Services;

using Microsoft.Extensions.DependencyInjection;
using Jenny.front.Services;

namespace Jenny_V2
{
    public static class Installer
    {
        public static void Install(IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddSingleton<SpeechRecognizerService>();
            services.AddSingleton<ChatGPTService>();

            services.AddTransient<KeywordService>();
            services.AddTransient<EventFactory>();
            services.AddTransient<VolumeService>();
            services.AddTransient<EventHandlerVolumeUp>();
            services.AddTransient<EventHandlerVolumeDown>();
            services.AddTransient<EventHandlerSetVolume>();
        }
    }
}
