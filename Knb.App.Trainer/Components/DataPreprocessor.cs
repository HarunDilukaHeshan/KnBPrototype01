using Knb.App.Trainer.Options;
using Knb.App.Trainer.Shared;
using Knb.DataStorage.Options;
using Knb.DataStorage.PTurnDataStorage;
using Knb.DataStorage.Shared.XmlStorage;
using Knb.DataStorage.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Knb.App.Trainer.Components
{
    class DataPreprocessor : IDataPreprocessor
    {
        protected IPTurnDataFilesStorage TurnDataFilesStorage { get; }
        protected IXmlDataFilesStorage XmlDataFilesStorage { get; }
        protected XmlDataFileNodeNames XmlDataFileNodeNames { get; }
        protected PTurnDataFileChunkOptions PTurnOptions { get; }
        protected bool Stopped { get; set; }

        public PreprocessorStopped OnPreprocessorStop { get; set; }
        public PreprocessorChanged OnPreprocessorChanged { get; set; }
        public DataPreprocessor(
            IPTurnDataFilesStorage turnDataFilesStorage, 
            IXmlDataFilesStorage xmlDataFilesStorage, 
            IOptions<XmlDataFileNodeNames> xmlDataFileNodeNames, 
            IOptions<PTurnDataFileChunkOptions> pTurnOptions)
        {
            TurnDataFilesStorage = turnDataFilesStorage ?? throw new ArgumentNullException();
            XmlDataFilesStorage = xmlDataFilesStorage ?? throw new ArgumentNullException();
            XmlDataFileNodeNames = xmlDataFileNodeNames.Value ?? throw new ArgumentNullException();
            PTurnOptions = pTurnOptions.Value ?? throw new ArgumentNullException();            
        }

        public async Task Start(Action<DataPreprocessorOptions> action)
        {
            Stopped = false;
            var options = new DataPreprocessorOptions();
            if (action == null) throw new ArgumentNullException();            
            action.Invoke(options);

            long completed = 0;
            FileInfo fileInfo = null;

            if (string.IsNullOrWhiteSpace(options.DataFileName)
                || string.IsNullOrWhiteSpace(options.PDataFileName))
                throw new ArgumentException();

            var chunks = XmlDataFilesStorage.GetAsyncChunks(options.DataFileName);

            var ptdList = new List<PTurnData>();
            await foreach(var chunk in chunks)
            {
                ptdList.AddRange(GetPTurnData(chunk));

                if (ptdList.Count >= PTurnOptions.BufferSize)
                {
                    await TurnDataFilesStorage.WriteAsync(options.PDataFileName, ptdList.ToArray());

                    fileInfo ??= await GetFileInfoAsync(options.DataFileName);
                    completed += PTurnOptions.BufferSize * GetUnitSize(ptdList[0]);

                    if (Stopped)
                    {
                        if (OnPreprocessorStop != null)
                            OnPreprocessorStop.Invoke(new PreprocessorTracker(completed, fileInfo.Length));
                        break;
                    }
                    else
                        if (OnPreprocessorChanged != null)
                            OnPreprocessorChanged.Invoke(new PreprocessorTracker(completed, fileInfo.Length));                    
                    ptdList.Clear();
                }
            }
        }

        public void Stop()
        {
            Stopped = true;
        }

        private async Task<FileInfo> GetFileInfoAsync(string fileName)
        {
            return await XmlDataFilesStorage.GetFileInfoAsync(fileName);
        }

        private long GetUnitSize(PTurnData pTurnData)
        {
            var uSize = pTurnData.ActiveCards.Length + pTurnData.InactiveCards.Length
                            + pTurnData.PrevTurns.Length + pTurnData.Hand.Length 
                            + pTurnData.FacedUpCards.Length;
            return uSize;
        }

        private PTurnData[] GetPTurnData(XmlDocument xmlDoc)
        {
            if (xmlDoc == null) throw new ArgumentNullException();

            var ptdList = new List<PTurnData>();
            var winnerId = xmlDoc.DocumentElement.GetAttribute(XmlDataFileNodeNames.PlayerIdAttr);
            var rounds = xmlDoc.DocumentElement.ChildNodes;

            if (string.IsNullOrWhiteSpace(winnerId)) throw new InvalidOperationException();

            foreach(XmlNode rd in rounds)
            {
                foreach(XmlElement td in rd.ChildNodes)
                {                    
                    var nodes = td.ChildNodes;
                    var playerId = td.GetAttribute(XmlDataFileNodeNames.PlayerIdAttr);

                    if (playerId != winnerId) continue;
                    if (nodes.Count != 5 || string.IsNullOrWhiteSpace(playerId)) throw new InvalidOperationException();

                    var facedUpCardsElm = nodes[0] ?? throw new InvalidOperationException();
                    var handElm = nodes[1] ?? throw new InvalidOperationException();
                    var prevTurnsElm = nodes[2] ?? throw new InvalidOperationException();
                    var activeCardsElm = nodes[3] ?? throw new InvalidOperationException();
                    var inactiveCardsElm = nodes[4] ?? throw new InvalidOperationException();

                    var ptd = new PTurnData
                    {
                        FacedUpCards = facedUpCardsElm.InnerXml,
                        Hand = handElm.InnerXml,
                        PrevTurns = prevTurnsElm.InnerXml,
                        ActiveCards = activeCardsElm.InnerXml,
                        InactiveCards = inactiveCardsElm.InnerXml
                    };

                    ptdList.Add(ptd);
                }
            }

            return ptdList.ToArray();
        }
    }
}
