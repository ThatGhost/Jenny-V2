﻿using Jenny_V2.EventHandlers.Core;
using Jenny_V2.EventHandlers;
using Jenny_V2.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Jenny_V2.Pages;
using Google.Api;
using Jenny_V2.Services.UI;
using Jenny_V2.Services.ResearchContext;
using Jenny_V2.Services.Core;

namespace Jenny_V2
{
    public static class Installer
    {
        public static void Install(IServiceCollection services)
        {
            services.AddSingleton<SpeechRecognizerService>();
            services.AddSingleton<ChatGPTService>();
            services.AddSingleton<TextToSpeechService>();
            services.AddSingleton<KeywordService>();
            services.AddSingleton<ZeroShotService>();
            services.AddSingleton<ResearchContextService>();
            services.AddSingleton<DictationService>();

            services.AddTransient<EventFactory>();
            services.AddTransient<VolumeService>();
            services.AddTransient<BrowserService>();
            services.AddTransient<FileService>();
            services.AddTransient<VoiceActivationService>();
            services.AddTransient<ChatService>();
            services.AddTransient<MarkDownService>();

            InstallEventHandlers(services);
            InstallUI(services);
        }

        private static void InstallUI(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainPage>();
            services.AddSingleton<DictationsPage>();
            services.AddSingleton<ChatPage>();
            services.AddSingleton<MainPageService>();
            services.AddSingleton<ChatPageService>();
        }

        private static void InstallEventHandlers(IServiceCollection services)
        {
            var handlerType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IEventHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in handlerType)
            {
                services.AddTransient(type);
            }
        }
    }
}
