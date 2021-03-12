using Knb.App.Simulator.GameData;
using Knb.App.Simulator.Shared.Components;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Knb.DataStorage.Shared.XmlStorage;
using Knb.DataStorage.Options;
using Microsoft.Extensions.Options;
using System.Xml;

namespace Knb.App.Simulator.Components
{
    class DataRecorder : IDataRecorder
    {
        protected IList<PlayData> PlayDataList = new List<PlayData>();
        protected IXmlDataFilesStorage XmlDataFilesStorage { get; }
        protected XmlDataFileNodeNames XmlDataFileNodeNames { get; }

        public DataRecorder(
            IXmlDataFilesStorage xmlDataFilesStorage,
            IOptions<XmlDataFileNodeNames> xmlDataFileNodeNames)
        {
            XmlDataFilesStorage = xmlDataFilesStorage ?? throw new ArgumentNullException(); ;
            XmlDataFileNodeNames = xmlDataFileNodeNames.Value ?? throw new ArgumentNullException();
        }

        public void Record(PlayData[] playDataArr)
        {
            ((List<PlayData>)PlayDataList).AddRange(playDataArr.ToArray());
        }

        public async Task SaveAsync(string fileName, bool validateDataFile = false)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException();

            if (XmlDataFilesStorage.Exists(fileName) && validateDataFile &&
                !(await XmlDataFilesStorage.ValidateXmlFile(fileName)))
                throw new InvalidOperationException("Invalid xml data format");

            var xmlStr = GetXmlString();
            await XmlDataFilesStorage.WriteXmlAsync(fileName, xmlStr);
            PlayDataList.Clear();
        }              

        protected virtual string GetXmlString()
        {
            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement(XmlDataFileNodeNames.Root);

            foreach (var pd in PlayDataList)
            {
                var play = xmlDoc.CreateElement(XmlDataFileNodeNames.PlayData);
                play.SetAttribute(XmlDataFileNodeNames.PlayerIdAttr, pd.WinnerId);

                foreach (var rd in pd.RoundData)
                {
                    var round = xmlDoc.CreateElement(XmlDataFileNodeNames.RoundData);
                    foreach (var td in rd.TurnDataArray)
                    {
                        var turn = xmlDoc.CreateElement(XmlDataFileNodeNames.TurnData);

                        var facedUpCardsStr = CardArrToString(td.FaceUpCards);
                        var handStr = CardArrToString(td.Hand);
                        var prevTurnsStr = TurnArrToString(td.PrevTurns);
                        var activeCardsStr = CardArrToString(td.ActiveCards);
                        var inactiveCardsStr = CardArrToString(td.InactiveCards);

                        var facedUpCardsElm = xmlDoc.CreateElement(XmlDataFileNodeNames.FacedUpCards);
                        var handStrElm = xmlDoc.CreateElement(XmlDataFileNodeNames.Hand);
                        var prevTurnsElm = xmlDoc.CreateElement(XmlDataFileNodeNames.PrevTurns);
                        var activeCardsElm = xmlDoc.CreateElement(XmlDataFileNodeNames.ActiveCards);
                        var inactiveCardsElm = xmlDoc.CreateElement(XmlDataFileNodeNames.InactiveCards);

                        facedUpCardsElm.AppendChild(xmlDoc.CreateTextNode(facedUpCardsStr));
                        handStrElm.AppendChild(xmlDoc.CreateTextNode(handStr));
                        prevTurnsElm.AppendChild(xmlDoc.CreateTextNode(prevTurnsStr));
                        activeCardsElm.AppendChild(xmlDoc.CreateTextNode(activeCardsStr));
                        inactiveCardsElm.AppendChild(xmlDoc.CreateTextNode(inactiveCardsStr));

                        turn.SetAttribute(XmlDataFileNodeNames.PlayerIdAttr, td.Player.PlayerID);
                        turn.AppendChild(facedUpCardsElm);
                        turn.AppendChild(handStrElm);
                        turn.AppendChild(prevTurnsElm);
                        turn.AppendChild(activeCardsElm);
                        turn.AppendChild(inactiveCardsElm);

                        round.AppendChild(turn);
                    }

                    play.AppendChild(round);
                }

                root.AppendChild(play);
            }

            xmlDoc.AppendChild(root);

            return xmlDoc.InnerXml;
        }

        private string CardArrToString(Card[] cardArr)
        {
            var str = cardArr.Select((card) => card.CardID );

            return string.Join(",", str);
        }

        private string TurnArrToString(TurnData[] turnDataArr)
        {
            var s = "";

            foreach (var td in turnDataArr)
            {
                var arr = td.FaceUpCards.Select(card => card.CardID);
                s += "[" + string.Join(",", arr) + "],";
            }

            if (s.Length > 1) s = s[0..^1];

            return s;
        }
    }
}
