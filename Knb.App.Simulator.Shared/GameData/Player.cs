using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knb.App.Simulator.GameData
{
    public class Player
    {
        public string PlayerID { get; }
        public int Count { get { return Hand.Count; } }
        protected IList<Card> Hand { get; }

        public Player(string id)
        {
            Hand = new List<Card>();
            PlayerID = id;
        }

        public void RemoveCard(Card card)
        {
            _ = card ?? throw new ArgumentNullException();
            if (!Hand.Remove(card)) throw new Exception("Item does not found");
        }

        public Card[] ToArray()
        {
            return Hand.ToArray();
        }
    }
}
