using Knb.App.Controllers;
using Knb.App.Simulator;
using Knb.App.Trainer.Shared;
using Knb.ServiceContainer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App
{
    [DependsOn(
        typeof(SimulatorModule), 
        typeof(TrainerModule))]
    public class AppModule : KnbModuleBase
    {
        public AppModule(IConfiguration configuration)
            : base(configuration)
        { }

        public override void ConfigureServices(IServiceCollection services)
        {
            // Simulator shared
            services.AddSingleton<SimulatorController>();
            services.AddSingleton<TrainerController>();
            services.AddSingleton<DataFilesStorageController>();
            services.AddSingleton<ModelFilesStorageController>();
            services.AddSingleton<PDataFilesStorageController>();
        }
    }
}
