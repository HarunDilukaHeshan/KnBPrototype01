using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Simulator.GameData
{
    public class CardPack : IEnumerable<Card>
    {
        protected Card[] Cards { get; }
        public int Count { get { return Cards.Length; } }

        public CardPack(Card[] cards)
        {
            var cardsList = new List<Card>();
            foreach (var card in cards) cardsList.Add(new Card(card.SuitName, card.CardName));
            Cards = cardsList.ToArray();
        }                

        public IEnumerator<Card> GetEnumerator() { return new CardPackEnumerator(Cards); }

        IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)GetEnumerator(); }

        public Card this[int index]
        {
            get => Cards[index];
        }
    }

    public class CardPackEnumerator : IEnumerator<Card>
    {
        protected Card[] Cards { get; set; }
        protected Card CurrentCard { get; set; }
        public Card Current { get { return CurrentCard; } }
        object IEnumerator.Current { get { return Current; } }
        protected long Position { get; set; } = -1;

        public CardPackEnumerator(Card[] cards)
        {
            Cards = cards;
        }

        public void Dispose()
        {
            Cards = new Card[0];
        }

        public bool MoveNext()
        {
            Position++;
            var canMove = Position < Cards.Length;
            if (canMove) CurrentCard = Cards[Position];
            return canMove;
        }

        public void Reset()
        {
            Position = -1;
            CurrentCard = null;
        }
    }
}
