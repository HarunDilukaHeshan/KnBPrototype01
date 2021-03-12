using Knb.App.Simulator;
using Knb.App.Simulator.Components;
using Knb.App.Simulator.Shared.Components;
using Knb.App.Simulator.Shared.GameData;
using Knb.App.Trainer.Components;
using Knb.DataStorage.Shared.XmlStorage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Knb.App.Controllers
{
    public class SimulatorController : ControllerBase
    {
        protected IServiceProvider ServiceProvider { get; }
        protected IKnbSimulator KnbSimulator { get; set; }
        protected CardSelectorOptions CardSelectorOptions { get; }
        public PeriodEventDel OnPeriodChange { get; set; }
        public PlayFinishedEventDel OnPlayFinished { get; set; }
        public SimulatorStoppedEventDel OnSimulatorStopped { get; set; }
        public SimulatorController(
            IServiceProvider serviceProvider, 
            IOptions<CardSelectorOptions> cardSelectorOptions)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException();
            CardSelectorOptions = cardSelectorOptions.Value ?? throw new ArgumentNullException();
        }

        public async Task Start(Action<KnbOptions> action)
        {
            var options = new KnbOptions();
            action.Invoke(options);

            if (string.IsNullOrWhiteSpace(options.FileName)) throw new ArgumentException();

            CardSelectorOptions.MlModelFileName = options.MlModelFileName;

            KnbSimulator = ServiceProvider.GetService<IKnbSimulator>() ?? throw new InvalidOperationException();

            KnbSimulator.OnPeriodChange = OnPeriodChange ?? KnbSimulator.OnPeriodChange;
            KnbSimulator.OnPlayFinished = OnPlayFinished ?? KnbSimulator.OnPlayFinished;
            KnbSimulator.OnSimulatorStopped = OnSimulatorStopped ?? KnbSimulator.OnSimulatorStopped;

            await KnbSimulator.Start(op =>
            {
                op.FileName = options.FileName;
                op.MlModelFileName = options.MlModelFileName;
                op.NoOfTimes = options.NoOfTimes;
                op.NoOfCardPacks = options.NoOfCardPacks;
                op.NoOfPlayers = options.NoOfPlayers;
                op.NoOfTimesPerPeriod = options.NoOfTimesPerPeriod;
            });
        }

        public void Stop()
        {
            KnbSimulator.Stop();
        }
    }
}
