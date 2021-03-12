using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knb.App.Simulator.GameData
{
    public class TurnData
    {
        protected readonly Player _player;
        protected readonly Card[] _facedUpCards;
        protected readonly Card[] _hand;
        protected readonly TurnData[] _prevTurns;
        protected readonly Card[] _activeCards;
        protected readonly Card[] _inactiveCards;

        public TurnData(Player player, Card[] faceUpCards, TurnData[] prevTurns, Card[] activeCards, Card[] inactiveCards)
        {
            _player = player;
            _facedUpCards = faceUpCards.ToArray();
            _hand = player.ToArray();
            _prevTurns = prevTurns.ToArray();
            _activeCards = activeCards.ToArray();
            _inactiveCards = inactiveCards.ToArray();
        }

        public Player Player { get { return _player; } }
        public Card[] FaceUpCards { get { return _facedUpCards.ToArray(); } }
        public Card[] Hand { get { return _hand.ToArray(); } }
        public TurnData[] PrevTurns { get { return _prevTurns.ToArray(); } }
        public Card[] ActiveCards { get { return _activeCards.ToArray(); } }
        public Card[] InactiveCards { get { return _inactiveCards.ToArray(); } }
    }
}
