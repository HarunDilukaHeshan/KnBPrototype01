using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Simulator.GameData
{
    public class Card
    {
        public Card(Suits suitName, Cards cardName)
        {
            SuitName = suitName;
            CardName = cardName;
            CardID = string.Format("{0}{1}", (char)suitName, cardName.ToString());
        }

        public string CardID { get; }
        public Suits SuitName { get; }
        public Cards CardName { get; }
    }
}
