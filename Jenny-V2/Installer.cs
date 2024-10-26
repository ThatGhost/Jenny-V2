using Jenny_V2.EventHandlers.Core;
using Jenny_V2.EventHandlers;
using Jenny_V2.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Jenny_V2.Pages;

namespace Jenny_V2
{
    public static class Installer
    {
        public static void Install(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainPage>();
            services.AddSingleton<DictationsPage>();

            services.AddSingleton<SpeechRecognizerService>();
            services.AddSingleton<ChatGPTService>();
            services.AddSingleton<TextToSpeechService>();
            services.AddSingleton<KeywordService>();
            services.AddSingleton<ZeroShotService>();
            services.AddSingleton<ResearchContextService>();

            services.AddTransient<EventFactory>();
            services.AddTransient<VolumeService>();
            services.AddTransient<BrowserService>();

            var handlerType = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IEventHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach(var type in handlerType)
            {
                services.AddTransient(type);
            }
        }
    }
}
