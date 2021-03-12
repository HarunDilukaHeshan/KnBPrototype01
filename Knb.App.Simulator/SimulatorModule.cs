using Knb.ServiceContainer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Knb.DataStorage;
using Knb.DataStorage.Storage;
using Knb.App.Simulator.Shared;
using Knb.App.Simulator.Components;
using Knb.App.Simulator.Shared.Components;
using Microsoft.Extensions.Options;
using Knb.App.Simulator.Shared.GameData;

namespace Knb.App.Simulator
{
    [DependsOn(
        typeof(SimulatorSharedModule), 
        typeof(DataStorageModule))]
    public class SimulatorModule : KnbModuleBase
    {
        public SimulatorModule(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            // Simulator shared
            services.AddSingleton<ICardHandOuter, CardHandOuter>();
            services.AddSingleton<ICardPackFactory, CardPackFactory>();
            services.AddSingleton<ICardPackShuffler, CardPackShuffler>();
            services.AddSingleton<IPlayManager, PlayManager>();
            services.AddSingleton<IRndGenerator, RndGenerator>();            
            services.AddSingleton<ISelectionRulesChecker, SelectionRulesChecker>();
            services.AddSingleton<IDataRecorder, DataRecorder>();
            services.AddSingleton<IKnbSimulator, KnBSimulator>();

            services.Configure<CardSelectorOptions>((options) => { });
            services.Configure<KnbOptions>((options) => { });

            services.AddSingleton<RndCardSelector>();
            services.AddSingleton<MLCardSelector>();

            services.AddTransient<ICardSelector>(provider => {
                var options = provider.GetService<IOptions<CardSelectorOptions>>().Value 
                    ?? throw new InvalidOperationException();

                if (string.IsNullOrWhiteSpace(options.MlModelFileName))
                    return provider.GetService<RndCardSelector>();
                else
                    return provider.GetService<MLCardSelector>();
            });
        }
    }
}
