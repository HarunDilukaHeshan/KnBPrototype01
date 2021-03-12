using Knb.App.Simulator.GameData;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Knb.App.Simulator.Components
{
    public class CardPackShuffler : ICardPackShuffler
    {
        protected IRndGenerator RndGenerator { get; }
        public CardPackShuffler(IRndGenerator rndGenerator)
        {
            RndGenerator = rndGenerator ?? throw new ArgumentNullException();
        }

        public void Shuffle(CardPack[] cardPacks)
        {
            _ = cardPacks ?? throw new ArgumentNullException();
            if (cardPacks.Length < 1) throw new ArgumentException();

            foreach (var cardPack in cardPacks)
            {
                var cards = cardPack.GetType()
                .GetProperty("Cards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(cardPack);

                if (!typeof(Card[]).IsInstanceOfType(cards))
                    throw new Exception();

                Shuffle(cards as Card[]);
            }
        }

        protected void Shuffle(Card[] cards)
        {
            var rndLen = RndGenerator.Next(52, 10000);
            for (var i = 0; i < rndLen; i++)
            {
                var indx = RndGenerator.Next(52);

                var firstE = cards[0];
                cards.SetValue(cards[indx], 0);
                cards.SetValue(firstE, indx);
            }
        }
    }
}
