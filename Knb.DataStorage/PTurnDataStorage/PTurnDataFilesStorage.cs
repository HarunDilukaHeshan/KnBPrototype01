using Knb.DataStorage.Options;
using Knb.DataStorage.Shared.PTurnDataStorage;
using Knb.DataStorage.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Knb.DataStorage.PTurnDataStorage
{
    class PTurnDataFilesStorage : IPTurnDataFilesStorage
    {
        protected IProcessedDataFilesStorage ProcessedDataFilesStorage { get; }
        protected PTurnDataFileChunkOptions Options { get; }
        protected ProcessedDataFileStructure PDataFileStructure { get; }
        public PTurnDataFilesStorage(
            IProcessedDataFilesStorage processedDataFilesStorage,
            IOptions<PTurnDataFileChunkOptions> options,
            IOptions<ProcessedDataFileStructure> pDataFileStructure)
        {
            ProcessedDataFilesStorage = processedDataFilesStorage ?? throw new ArgumentNullException();
            Options = options.Value ?? throw new ArgumentNullException();
            PDataFileStructure = pDataFileStructure.Value ?? throw new ArgumentNullException();
        }

        public async Task<bool> CreateDataFileAsync(string fileName)
        {
            var columns = string.Join('\t', PDataFileStructure.Columns);
            var result = await ProcessedDataFilesStorage.CreateFileAsync(fileName, Encoding.UTF8.GetBytes(columns));

            return result;
        }

        public async Task<Storage.FileInfo[]> GetFilesInfoAsync()
        {
            return await ProcessedDataFilesStorage.GetFilesInfo();
        }

        public IEnumerable<PTurnData> GetChunks(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException();

            var chunks = ProcessedDataFilesStorage.GetChunks(fileName);
            return new PTurnDataFilesEnum(chunks, Options.BufferSize, PDataFileStructure);
        }

        public IAsyncEnumerable<PTurnData[]> GetAsyncChunks(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException();

            var chunks = ProcessedDataFilesStorage.GetAsyncChunks(fileName);
            return new PTurnDataFilesAsyncEnum(chunks, Options.BufferSize, PDataFileStructure);
        }

        public async Task WriteAsync(string fileName, PTurnData[] pTurnDataArr)
        {
            if (string.IsNullOrWhiteSpace(fileName) ||
                pTurnDataArr == null || pTurnDataArr.Length == 0) throw new ArgumentException();

            if (!ProcessedDataFilesStorage.Exists(fileName)) await CreateDataFileAsync(fileName);

            using var st = ProcessedDataFilesStorage.GetWriteStream(fileName);
            st.Position = st.Length;

            foreach (var pt in pTurnDataArr)
            {
                var s = '\t';
                var str = string.Concat(Environment.NewLine, pt.FacedUpCards, s, pt.Hand, s, pt.PrevTurns,
                    s, pt.ActiveCards, s, pt.InactiveCards);
                await st.WriteAsync(Encoding.UTF8.GetBytes(str));
            }

            await st.DisposeAsync();
        }

        public bool Exists(string fileName)
        {
            return ProcessedDataFilesStorage.Exists(fileName);
        }
    }
}
