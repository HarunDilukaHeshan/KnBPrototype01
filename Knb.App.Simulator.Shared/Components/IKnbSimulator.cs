using Knb.App.Simulator.GameData;
using Knb.App.Simulator.Shared.GameData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.App.Simulator.Components
{
    public delegate void PeriodEventDel(int period, int noOfPeriods);
    public delegate void PlayFinishedEventDel(NoOfTimes noOfTimes, NoOfTimesPerPeriod noOfTimesPerPeriod);
    public delegate void SimulatorStoppedEventDel();
    public interface IKnbSimulator 
    {
        Task Start(Action<KnbOptions> action);
        PeriodEventDel OnPeriodChange { get; set; }
        PlayFinishedEventDel OnPlayFinished { get; set; }
        SimulatorStoppedEventDel OnSimulatorStopped { get; set; }
        void Stop();
    }
}
