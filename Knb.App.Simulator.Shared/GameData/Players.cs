using Knb.App.Simulator.GameData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Simulator.GameData
{
    public class Players : IEnumerable<Player>
    {
        protected IList<Player> PlayersList { get; }
        public int Count { get { return PlayersList.Count; } }
        protected Players(NoOfPlayers noOfPlayers)
        {
            PlayersList = new List<Player>();
            for (var i = 0; i < (int)noOfPlayers; i++)
                PlayersList.Add(new Player(string.Format("Player{0}", i)));
        }

        public static Players Create(NoOfPlayers noOfPlayers)
        {
            return new Players(noOfPlayers);
        }

        public void RemovePlayer(Player player)
        {
            _ = player ?? throw new ArgumentNullException();

            if (!PlayersList.Remove(player)) throw new InvalidOperationException("Player does not found");
        }

        public IEnumerator<Player> GetEnumerator()
        {
            return new PlayersEnumerator(PlayersList);
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public Player this[int index] => PlayersList[index];

    }

    class PlayersEnumerator : IEnumerator<Player>
    {
        protected readonly int _count = 0;

        protected int _position = -1;

        protected IList<Player> Players { get; }

        protected Player CurrentPlayer { get; set; }

        public Player Current { get { return CurrentPlayer; } }

        object IEnumerator.Current { get { return Current; } }

        public PlayersEnumerator(IList<Player> players)
        {
            Players = players;
            _count = players.Count;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (_count != Players.Count) throw new InvalidOperationException("The collection has modified");

            _position++;
            var canMove = _position < _count;
            if (canMove) CurrentPlayer = Players[_position];
            return canMove;
        }

        public void Reset()
        {
            if (_count != Players.Count) throw new InvalidOperationException("The collection has modified");

            _position = -1;
            CurrentPlayer = null;
        }
    }
}
