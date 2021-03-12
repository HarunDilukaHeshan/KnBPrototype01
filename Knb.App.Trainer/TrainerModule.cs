using Knb.App.Trainer.Components;
using Knb.ServiceContainer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Trainer.Shared
{
    [DependsOn(
        typeof(TrainerSharedModule))]
    public class TrainerModule : KnbModuleBase
    {
        public TrainerModule(IConfiguration configuration)
            : base(configuration)
        { }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDataPreprocessor, DataPreprocessor>();
            services.AddSingleton<TrainerBase, SdcaMaximumEntropyTrainer>();
            services.AddSingleton<IKnbTrainer, KnbTrainer>();
        }
    }
}
