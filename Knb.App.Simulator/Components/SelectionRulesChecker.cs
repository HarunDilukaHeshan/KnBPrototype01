using Knb.App.Simulator.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knb.App.Simulator.Components
{
    public class SelectionRulesChecker : ISelectionRulesChecker
    {
        public bool Check(Card[] selection, Card[] hand, TurnData[] currentRound)
        {
            if (selection == null || hand == null || currentRound == null)
                throw new ArgumentNullException();

            if (selection.Length == 0) return true;

            if (selection.Length != hand.Count(card => card.CardName == selection[0].CardName))
                return false;

            var prevTurn = currentRound.LastOrDefault(e => e.FaceUpCards.Length > 0);

            if (prevTurn == null) return true;

            if (prevTurn.FaceUpCards.Length != selection.Length) return false;

            return ((int)selection[0].CardName > (int)prevTurn.FaceUpCards[0].CardName);
        }
    }
}
