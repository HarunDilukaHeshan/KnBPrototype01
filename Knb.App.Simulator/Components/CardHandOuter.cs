using Knb.App.Simulator.GameData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Simulator.Components
{
    public class CardHandOuter : ICardHandOuter
    {
        public CardHandOuter()
        {

        }

        public void HandOut(CardPack[] cardPacks, Players players)
        {

            foreach (var cardPack in cardPacks)
                for (int i = 0; i < cardPack.Count;)
                    foreach (var player in players)
                        if (i >= cardPack.Count)
                            break;
                        else
                            AddToHand(player, cardPack[i++]);
        }

        protected void AddToHand(Player player, Card card)
        {
            IList<Card> hand = player.GetType()
                .GetProperty("Hand", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(player) as IList<Card>;

            if (!typeof(IList<Card>).IsInstanceOfType(hand)) throw new InvalidOperationException();

            hand.Add(card);
        }
    }
}
