using Knb.App.Simulator.GameData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Simulator.Components
{
    class CardPackFactory : ICardPackFactory
    {
        public CardPack[] Create(NoOfCardPacks noOfCardPacks)
        {
            var cardPacks = new List<CardPack>();

            for (int i = 0; i < (int)noOfCardPacks; i++)
                cardPacks.Add(CreatePack());

            return cardPacks.ToArray();
        }

        protected CardPack CreatePack()
        {
            var cardsList = new List<Card>();
            foreach (var suit in Enum.GetValues(typeof(Suits)))
            {
                var s = (Suits)suit;
                foreach (var card in Enum.GetValues(typeof(Cards)))
                {
                    var c = (Cards)card;
                    cardsList.Add(new Card(s, c));
                }
            }

            return new CardPack(cardsList.ToArray());
        }
    }
}
