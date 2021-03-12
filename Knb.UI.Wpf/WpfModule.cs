using Knb.App;
using Knb.ServiceContainer;
using Knb.UI.Wpf.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.UI.Wpf
{
    [DependsOn(
        typeof(AppModule))]
    public class WpfModule : KnbModuleBase
    {
        public WpfModule(IConfiguration configuration)
        : base(configuration)
        { }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SimulatorPage>();
            services.AddSingleton<TrainerPage>();
            services.AddSingleton<FilesPage>();
        }
    }
}
