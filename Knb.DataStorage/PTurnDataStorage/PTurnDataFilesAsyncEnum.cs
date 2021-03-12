using Knb.DataStorage.Shared.PTurnDataStorage;
using Knb.DataStorage.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Knb.DataStorage.PTurnDataStorage
{
    class PTurnDataFilesAsyncEnum : IAsyncEnumerable<PTurnData[]>
    {
        protected IAsyncEnumerable<byte[]> Chunks { get; }
        protected int ChunkSize { get; }
        protected ProcessedDataFileStructure PdfStructure { get; }
        public PTurnDataFilesAsyncEnum(
            IAsyncEnumerable<byte[]> chunks, 
            int chunkSize,
            ProcessedDataFileStructure pdfStructure)
        {
            if (chunkSize < 1) throw new ArgumentException();
            Chunks = chunks;
            ChunkSize = chunkSize;
            PdfStructure = pdfStructure ?? throw new ArgumentNullException();
        }

        public IAsyncEnumerator<PTurnData[]> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new PTurnDataFilesAsyncEnumerator(Chunks.GetAsyncEnumerator(), ChunkSize, PdfStructure);
        }
    }
}
