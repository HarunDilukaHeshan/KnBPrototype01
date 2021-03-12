using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Xml;

namespace Trainer
{
    public interface IDataStorage
    {
        IAsyncEnumerable<XmlElement> GameDataChunks { get; }
    }

    public class DataStorage : IDataStorage
    {
        protected GameDataStorageOptions Options { get; }
        protected XmlGameDataChunks DataChunks { get; }

        public DataStorage(GameDataStorageOptions options)
        {
            Options = options;
            DataChunks = new XmlGameDataChunks(options);
        }

        public IAsyncEnumerable<XmlElement> GameDataChunks { get { return DataChunks; } }
    }


    public class XmlGameDataChunks : IAsyncEnumerable<XmlElement>
    {
        protected GameDataStorageOptions Options { get; }
        protected XmlReader CurrentReader { get; set; }

        public XmlGameDataChunks(GameDataStorageOptions options)
        {
            Options = options;
        }

        public IAsyncEnumerator<XmlElement> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            if (CurrentReader != null) CurrentReader.Dispose();
            CurrentReader = GetReader();
            return new XmlGameDataEnumerator(CurrentReader);
        }

        private XmlReader GetReader()
        {
            var path = Path.Join(Options.FilePath, Options.FileName);
            var settings = GetSettings();
            var reader = XmlReader.Create(path, settings);

            return reader;
        }

        private XmlReaderSettings GetSettings()
        {
            var settings = new XmlReaderSettings
            {
                Async = true,
                ConformanceLevel = ConformanceLevel.Auto,
                IgnoreWhitespace = true
            };

            return settings;
        }
    }

    class XmlGameDataEnumerator : IAsyncEnumerator<XmlElement>
    {
        private XmlElement _current;

        protected XmlReader Reader { get; }
        public XmlElement Current { get { return _current; } }

        public XmlGameDataEnumerator(XmlReader reader)
        {
            Reader = reader ?? throw new ArgumentNullException();            
            Reader.ReadToNextSibling("xs:game-data");
            Reader.ReadToDescendant("play-data");
        }

        public async ValueTask DisposeAsync()
        {
            await Task.Run(() => {
                Reader.Close();
                Reader.Dispose();
            });
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (Reader.Name != "play-data") return false;

            var result = Reader.ReadToNextSibling("play-data");

            if (result)
            {
                var cnt = await Reader.ReadOuterXmlAsync();
                var doc = new XmlDocument();
                doc.LoadXml(cnt);
                _current = doc.DocumentElement;
            }

            return result;
        }
    }

    public class GameDataStorageOptions
    { 
        public string FilePath { get; set; } = "gameData";
        public string FileName { get; set; } = "game-data.dat";
    }

    public class PlayData
    {
        private readonly RoundData[] _roundDataArray;

        public PlayData(RoundData[] roundDataArray)
        {
            _roundDataArray = roundDataArray.ToArray();
        }

        public RoundData[] RoundData { get { return _roundDataArray.ToArray(); } }
    }

    public class RoundData
    {
        private readonly TurnData[] _turnDataArr;
        public RoundData(TurnData[] turnDataArray)
        {
            _turnDataArr = turnDataArray.ToArray();
        }

        public TurnData[] TurnDataArray { get { return _turnDataArr.ToArray(); } }
    }

    public class TurnData
    {
        public TurnData(
            string playerId, 
            string facedUpCards, 
            string hand, 
            string prevTurns, 
            string activeCards, 
            string inactiveCards)
        {
           // if (string.IsNullOrWhiteSpace(playerId)) throw new ArgumentException();

            PlayerId = playerId ?? throw new ArgumentNullException();
            FacedUpCards = facedUpCards ?? throw new ArgumentNullException();
            Hand = hand ?? throw new ArgumentNullException();
            PrevTurns = prevTurns ?? throw new ArgumentNullException();
            ActiveCards = activeCards ?? throw new ArgumentNullException();
            InactiveCards = inactiveCards ?? throw new ArgumentNullException();
        }

        public string PlayerId { get; }
        public string FacedUpCards { get; }
        public string Hand { get; }
        public string PrevTurns { get; }
        public string ActiveCards { get; }
        public string InactiveCards { get; }
    }


    public class PlayDataRepository : IAsyncEnumerable<PlayData>
    {
        protected IDataStorage DataStorage { get; }
        public PlayDataRepository(IDataStorage dataStorage)
        {
            DataStorage = dataStorage;
        }

        public IAsyncEnumerator<PlayData> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var chunks = DataStorage.GameDataChunks;
            return new PlayDataRepoEnumerator(chunks.GetAsyncEnumerator());
        }
    }

    public class PlayDataRepoEnumerator : IAsyncEnumerator<PlayData>
    {
        protected IAsyncEnumerator<XmlElement> Enumerator { get; }
        public PlayData Current { get; protected set; }

        public PlayDataRepoEnumerator(IAsyncEnumerator<XmlElement> enumerable)
        {
            Enumerator = enumerable ?? throw new ArgumentNullException();
        }

        public async ValueTask DisposeAsync()
        {
            await Enumerator.DisposeAsync();
            Current = null;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            var result = await Enumerator.MoveNextAsync();

            if (result)
            {
                var curElm = Enumerator.Current;
                Current = MapToPlayData(curElm);
            }

            return result;
        }

        protected PlayData MapToPlayData(XmlElement xmlElement)
        {
            _ = xmlElement ?? throw new ArgumentNullException();
            if (xmlElement.Name != "play-data") throw new ArgumentException();

            var roundDataList = new List<RoundData>();

            foreach (XmlElement round in xmlElement)
            {
                var turnDataList = new List<TurnData>();
                foreach(XmlElement turn in round)
                {
                    if (turn.ChildNodes.Count < 5) throw new InvalidOperationException("");

                    var playerId = turn.GetAttribute("playerId");

                    var facedUpCardsElm = turn["faced-up-cards"] ?? throw new Exception();
                    var handElm = turn["hand"] ?? throw new Exception();
                    var prevTurnsElm = turn["prev-turns"] ?? throw new Exception();
                    var activeCardsElm = turn["active-cards"] ?? throw new Exception();
                    var inactiveCardsElm = turn["inactive-cards"] ?? throw new Exception();

                    var turnData = new TurnData(
                        playerId, 
                        facedUpCardsElm.InnerXml, 
                        handElm.InnerXml, 
                        prevTurnsElm.InnerXml, 
                        activeCardsElm.InnerXml, 
                        inactiveCardsElm.InnerXml);

                    turnDataList.Add(turnData);
                }

                roundDataList.Add(new RoundData(turnDataList.ToArray()));
            }

            return new PlayData(roundDataList.ToArray());
        }
    }

}

