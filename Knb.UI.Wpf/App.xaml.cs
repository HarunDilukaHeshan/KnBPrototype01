using Knb.ServiceContainer.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Knb.UI.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected IHost AppHost;
        public App()
        {
            Init();
        }

        protected void Init()
        {
            AppHost = CreateDefaultHostBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddServiceContainers(context.Configuration);
                    services.AddSingleton<MainWindow>();
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("AppSettings.json", optional: false, reloadOnChange: false);
                })
                .ConfigureLogging((context, logging) =>
                {
                    var logger = new LoggerConfiguration()
                        .ReadFrom
                        .Configuration(context.Configuration)
                        .CreateLogger();

                    logging.AddSerilog(logger);
                })
                .Build();
        }

        static IHostBuilder CreateDefaultHostBuilder() =>
            Host.CreateDefaultBuilder();

        protected async void StartAsync(object sender, StartupEventArgs e)
        {
            await AppHost.StartAsync();
            AppHost.Services
                .GetService<MainWindow>()
                .Show();
        }

        protected async void StopAsync(object sender, ExitEventArgs e)
        {
            using (AppHost)
            {
                await AppHost.StopAsync();
            }
        }
    }    
}
