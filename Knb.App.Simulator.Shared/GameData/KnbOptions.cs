using Knb.App.Simulator.GameData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Simulator.Shared.GameData
{
    public class KnbOptions
    {
        public NoOfTimes NoOfTimes { get; set; } = NoOfTimes.Thousand;
        public NoOfTimesPerPeriod NoOfTimesPerPeriod { get; set; } = NoOfTimesPerPeriod.Hundren;
        public NoOfPlayers NoOfPlayers { get; set; } = NoOfPlayers.Four;
        public NoOfCardPacks NoOfCardPacks { get; set; } = NoOfCardPacks.One;
        public string FileName { get; set; } = string.Empty;
        public string MlModelFileName { get; set; } = string.Empty;
    }
}
