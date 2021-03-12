using Knb.App.Simulator.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knb.App.Simulator.Components
{
    public class RndCardSelector : ICardSelector
    {
        protected IRndGenerator RndGenerator { get; }

        public RndCardSelector(IRndGenerator rndGenerator)
        {
            RndGenerator = rndGenerator;
        }

        public Card[] Select(Card[] hand, TurnData[] currentRound, IList<Card> activeCards, IList<Card> inactiveCards)
        {
            var patterns = hand.GroupBy(c => (int)c.CardName);
            var prevTurn = currentRound.LastOrDefault(e => e.FaceUpCards.Length != 0);
            var rndMax = 10000;

            if (prevTurn == null)
            {
                var f = patterns.FirstOrDefault();
                return (f == null) ? new Card[0] : f.ToArray();
            }
            else
            {
                var rndVal = RndGenerator.Next(rndMax);
                if (rndVal < rndMax / 2) return new Card[0];

                var c = patterns.Where(e => (int)e.First().CardName > (int)prevTurn.FaceUpCards.First().CardName);
                if (c.Count() > 0)
                {
                    var preTurnLen = prevTurn.FaceUpCards.Length;
                    if (preTurnLen == 1)
                    {
                        return new Card[] { c.First().First() };
                    }
                    else
                    {
                        var g = c.Where(e => e.Count() == preTurnLen);
                        if (g.Count() == 0)
                        {
                            return new Card[0];
                        }
                        else
                            return g.First().ToArray();
                    }
                }
                else
                    return new Card[0];
            }
        }
    }
}
