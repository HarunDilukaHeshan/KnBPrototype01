using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knb.App.Simulator.GameData
{
    public class PlayData
    {
        protected RoundData[] RoundDataArr { get; }
        
        public PlayData(RoundData[] rounds, string winnerId)
        {
            if (rounds == null) throw new ArgumentNullException();
            if (rounds.Length == 0) throw new ArgumentException();
            if (string.IsNullOrWhiteSpace(winnerId)) throw new ArgumentNullException();

            RoundDataArr = rounds;
            WinnerId = winnerId;
        }

        public RoundData[] RoundData { get { return RoundDataArr.ToArray(); } }
        public string WinnerId { get; }
    }
}
