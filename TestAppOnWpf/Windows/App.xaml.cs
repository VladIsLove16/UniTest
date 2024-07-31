using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using TestAppOnWpf.FileSaveSystem;
using TestAppOnWpf.SaveLoaderSystem;
namespace TestAppOnWpf
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        private readonly IHost _host;
        public App()
        {
            //var serviceCollection = new ServiceCollection();
            //ConfigureServices(serviceCollection);
            //_serviceProvider = serviceCollection.BuildServiceProvider();
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                })
                .Build();
        }
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ISaveService, JsonSaveService>();
            services.AddSingleton<IRepository, Repository>();
            services.AddSingleton<IStudentCollection, StudentDictCollection>();
            services.AddSingleton<SaveLoadersManager>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            ServiceProvider = _host.Services;
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
