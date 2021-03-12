using Knb.App.Simulator.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knb.App.Simulator.Components
{
    class PlayManager : IPlayManager
    {
        public CardPack[] CardPacks { get; set; }

        protected ISelectionRulesChecker SelectionRulesChecker { get; }
        protected ICardHandOuter CardHandOuter { get; }
        protected ICardSelector CardSelector { get; }
        protected ICardPackShuffler CardPackShuffler { get; }

        public PlayManager(
            ISelectionRulesChecker selectionRulesChecker,
            ICardHandOuter cardHandOuter,
            ICardSelector cardSelector,
            ICardPackShuffler cardPackShuffler)
        {
            CardHandOuter = cardHandOuter;
            CardSelector = cardSelector;
            CardPackShuffler = cardPackShuffler;
            SelectionRulesChecker = selectionRulesChecker;
        }

        public async Task<PlayData> PlayAsync(Players players)
        {
            _ = players ?? throw new ArgumentNullException();
            if (players.Count < 4) throw new ArgumentException("Number of players cannot be less than 4");

            _ = CardPacks ?? throw new NullReferenceException("Card packs cannot be null");
            if (CardPacks.Length == 0) throw new InvalidOperationException("CardPacks cannot be null");

            var playerId = "";
            IList<RoundData> gameData = new List<RoundData>();

            await Task.Run(() =>
            {

                CardPackShuffler.Shuffle(CardPacks);
                CardHandOuter.HandOut(CardPacks, players);

                IList<Card> activeCards = new List<Card>();
                IList<Card> inactiveCards = new List<Card>();

                int playerIndex = 0;
                bool isEnd = false;

                foreach (var cp in CardPacks)
                    ((List<Card>)activeCards).AddRange(cp);

                while (!isEnd)
                {
                    IList<TurnData> round = new List<TurnData>();
                    int errCount = 0;
                    int passedCount = 0;
                    bool endOfRound = false;

                    while (!endOfRound)
                    {
                        for (int i = playerIndex; i < players.Count; i++)
                        {
                            var player = players[i];
                            var selectedCards = CardSelector.Select(player.ToArray(), round.ToArray(), activeCards, inactiveCards);
                            selectedCards = SelectionRulesChecker.Check(selectedCards, player.ToArray(), round.ToArray()) ? selectedCards : new Card[0];

                            passedCount = (selectedCards.Length == 0) ? passedCount + 1 : 0;

                            foreach (var card in selectedCards)
                            {
                                inactiveCards.Add(card);
                                activeCards.Remove(card);
                                player.RemoveCard(card);
                            }

                            if (player.ToArray().Length == 0)
                            {
                                if (playerId == "") playerId = player.PlayerID;
                                players.RemovePlayer(player);
                            }

                            round.Add(new TurnData(player, selectedCards.ToArray(), round.ToArray(), activeCards.ToArray(), inactiveCards.ToArray()));

                            if (passedCount == players.Count - 1)
                            {
                                playerIndex = (i == passedCount) ? 0 : i + 1;
                                isEnd = (players.Count == 1);
                                endOfRound = true;
                                break;
                            }

                            playerIndex = 0;
                        }

                        errCount++;

                        if (errCount > 99)
                        {
                            throw new OperationCanceledException();
                        }
                    }

                    gameData.Add(new RoundData(round.ToArray()));
                }
                
                gameData.Add(new RoundData(new TurnData[] { new TurnData(players[0], new Card[0], new TurnData[0], new Card[0], new Card[0]) }));

            });

            return new PlayData(gameData.ToArray(), playerId);
        }
    }
}

