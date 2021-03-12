using Knb.App.Simulator.GameData;
using Knb.DataStorage.Storage;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knb.App.Simulator.Components
{
    class MLCardSelector : ICardSelector, IDisposable
    {
        protected IMLModelFilesStorage ModelFilesStorage { get; }
        protected CardSelectorOptions Options { get; }
        protected PredictionEngine<PTurnData, FacedUpCardPrediction> PredictionEngine { get; }
        public MLCardSelector(            
            IMLModelFilesStorage modelFilesStorage, 
            IOptions<CardSelectorOptions> options)
        {
            ModelFilesStorage = modelFilesStorage ?? throw new ArgumentNullException();
            Options = options.Value ?? throw new ArgumentNullException();
            PredictionEngine = LoadPredictionEngine() ?? throw new InvalidOperationException();
        }

        public Card[] Select(Card[] hand, TurnData[] currentRound, IList<Card> activeCards, IList<Card> inactiveCards)
        {
            var pdata = GetTurnData(hand, currentRound, activeCards, inactiveCards);
            var prediction = PredictionEngine.Predict(pdata);
            var cardStrArr = (string.IsNullOrWhiteSpace(prediction.FacedUpCards)) ? new string[0] : prediction.FacedUpCards.Split(",");
            IList<Card> cards = new List<Card>();

            foreach(var cardStr in cardStrArr)
            {
                var c = hand.Where(e => e.CardID == cardStr).ToArray();
                if (c.Length > 0)
                    cards.Add(c[0]);
            }

            if (cardStrArr.Length == 1)
            {

            }
            
            return (cardStrArr.Length == cards.Count)? cards.ToArray() : new Card[0];
        }

        protected virtual PredictionEngine<PTurnData, FacedUpCardPrediction> LoadPredictionEngine()
        {
            if (string.IsNullOrWhiteSpace(Options.MlModelFileName)) throw new InvalidOperationException();

            var mlContext = new MLContext();
            using var modelSt = ModelFilesStorage.GetReadStream(Options.MlModelFileName);
            ITransformer loadedModel = mlContext.Model.Load(modelSt, out var modelInputSchema);
            var predictionEngine = mlContext.Model.CreatePredictionEngine<PTurnData, FacedUpCardPrediction>(loadedModel);
            modelSt.Dispose();

            return predictionEngine;
        }

        private PTurnData GetTurnData(Card[] hand, TurnData[] currentRound, IList<Card> activeCards, IList<Card> inactiveCards)
        {
            var handStr = CardsArrToStr(hand);
            var turnDataStr = TurnDataArrToStr(currentRound);
            var activeCardsStr = CardsArrToStr(activeCards.ToArray());
            var inactiveCardsStr = CardsArrToStr(inactiveCards.ToArray());

            return new PTurnData
            {
                Hand = handStr,
                PrevTurns = turnDataStr,
                ActiveCards = activeCardsStr,
                InactiveCards = inactiveCardsStr
            };
        }
        
        private string TurnDataArrToStr(TurnData[] turnDataArr)
        {
            var str = "";
            foreach (var td in turnDataArr)
                str += "[" + CardsArrToStr(td.FaceUpCards) + "],";
            return (turnDataArr.Length > 0) ? str[0..^1] : "";
        }

        private string CardsArrToStr(Card[] cards)
        {
            var str = "";

            foreach (var card in cards)
                str += card.CardID + ",";

            return (cards.Length > 0) ? str[0..^1] : "";
        }

        public void Dispose()
        {
            PredictionEngine.Dispose();
        }
    }

    class FacedUpCardPrediction
    {
        [ColumnName("PredictedLabel")]
        public string FacedUpCards { get; set; }
    }
}
