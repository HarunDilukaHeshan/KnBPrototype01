using Knb.App.Simulator.GameData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Simulator.Components
{
    public interface ICardSelector
    {
        Card[] Select(Card[] hand, TurnData[] currentRound, IList<Card> activeCards, IList<Card> inactiveCards);
    }
}