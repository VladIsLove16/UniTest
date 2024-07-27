using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using TestAppOnWpf.FileSaveSystem;
using TestAppOnWpf;
using TestAppOnWpf.SaveLoaderSystem;
public class Program
{
    [STAThread]
    public static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<App>();
                services.AddSingleton<MainWindow>();
                // добавляем сервис IDateService
                //services.AddSingleton<ISaveService, XmlSaveService>();
                //services.AddSingleton<IRepository,Repository>();
                //services.AddSingleton<IStudentCollection, StudentDictCollection>();
            })
            .Build();
        var app = host.Services.GetService<App>();
        app?.Run();
    }
}