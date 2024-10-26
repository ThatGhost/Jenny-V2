using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Jenny_V2.Services;
using Jenny_V2.EventHandlers;
using Jenny_V2.EventHandlers.Core;
using Jenny_V2.Pages;

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

        [STAThread]
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }
    }
}
