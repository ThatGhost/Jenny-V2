using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Jenny_V2.Services;

namespace Jenny_V2
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register your services and ViewModels here
            services.AddTransient<MainWindow>();
            services.AddSingleton<SpeechRecognizerService>();
            services.AddSingleton<ChatGPTService>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
