using Knb.App.Simulator.GameData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Simulator.Components
{
    public interface ISelectionRulesChecker 
    { 
        bool Check(Card[] selection, Card[] hand, TurnData[] currentRound); 
    }
}
