using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knb.App.Simulator.GameData
{
    public class RoundData
    {
        protected TurnData[] TurnDataArr { get; }
        public RoundData(TurnData[] turns)
        {
            if (turns == null) throw new ArgumentNullException();
            TurnDataArr = turns.ToArray();
        }

        public TurnData[] TurnDataArray { get { return TurnDataArr.ToArray(); } }
    }
}
