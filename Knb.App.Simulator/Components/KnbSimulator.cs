using Knb.App.Simulator.GameData;
using Knb.App.Simulator.Shared.Components;
using Knb.App.Simulator.Shared.GameData;
using Knb.DataStorage.Shared.XmlStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.App.Simulator.Components
{    
    public class KnBSimulator : IKnbSimulator
    {
        protected KnbOptions Options { get; set; }
        protected IPlayManager PlayManager { get; }
        protected ICardPackFactory CardPackFactory { get; }
        protected IDataRecorder DataRecorder { get; }
        protected int CurrentPeriod { get; set; }
        protected bool WantToStop { get; set; } = false;
        public bool HasStarted { get; protected set; } = false;

        public KnBSimulator(
            IPlayManager playManager,
            ICardPackFactory cardPackFactory, 
            IDataRecorder dataRecorder)
        {
            PlayManager = playManager ?? throw new ArgumentNullException();
            CardPackFactory = cardPackFactory ?? throw new ArgumentNullException();
            DataRecorder = dataRecorder ?? throw new ArgumentNullException();
        }

        public PeriodEventDel OnPeriodChange { get; set; }
        public PlayFinishedEventDel OnPlayFinished { get; set; }
        public SimulatorStoppedEventDel OnSimulatorStopped { get; set; }
        public void Stop()
        {
            WantToStop = true;
        }             

        public async Task Start(Action<KnbOptions> action)
        {
            if (action == null) throw new ArgumentNullException();
            var options = new KnbOptions();
            action.Invoke(options);
            Options = options;

            if (string.IsNullOrWhiteSpace(options.FileName)) throw new ArgumentException();

            try
            {
                await StartSimulator();
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("User may not have permission to access the required file");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                throw new System.IO.FileNotFoundException("File does not found");
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured");
            }
        }

        private async Task StartSimulator()
        {
            if (HasStarted) return;

            var fileName = Options.FileName;

            HasStarted = true;
            WantToStop = false;

            var noOfPeriods = Convert.ToInt32((int)Options.NoOfTimes / (int)Options.NoOfTimesPerPeriod);
            var cardPacks = CardPackFactory.Create(Options.NoOfCardPacks);

            PlayManager.CardPacks = cardPacks;

            for (int period = CurrentPeriod; period < noOfPeriods; period++)
            {
                for (int i = 0; i < (int)Options.NoOfTimesPerPeriod; i++)
                {
                    var players = Players.Create(Options.NoOfPlayers);
                    var data = await PlayManager.PlayAsync(players);

                    DataRecorder.Record(new[] { data });
                }

                await DataRecorder.SaveAsync(fileName, period == 0);
                CurrentPeriod = period;
                if (OnPeriodChange != null) OnPeriodChange.Invoke(period, noOfPeriods);
                if (WantToStop) break;
            }

            CurrentPeriod = (WantToStop) ? 0 : CurrentPeriod;
            if (OnPlayFinished != null && WantToStop == false) OnPlayFinished.Invoke(Options.NoOfTimes, Options.NoOfTimesPerPeriod);
            if (WantToStop && OnSimulatorStopped != null) OnSimulatorStopped.Invoke();
            HasStarted = false;
        }
    }
}
