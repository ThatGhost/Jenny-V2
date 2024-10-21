using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Jenny_V2.Services;
using Jenny_V2.EventHandlers;
using Jenny_V2.EventHandlers.Core;

namespace Jenny_V2
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            Installer.Install(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
