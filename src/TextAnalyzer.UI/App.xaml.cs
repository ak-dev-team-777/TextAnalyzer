using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using TextAnalyzer.Core.Services.WordExtractor;

namespace TextAnalyzer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IWordExtractorService, WordExtractorService>();
            serviceCollection.AddSingleton<MainWindow>();
        }
    }
}
