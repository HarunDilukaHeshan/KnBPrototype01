using Knb.DataStorage.Shared.PTurnDataStorage;
using Knb.DataStorage.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Knb.DataStorage.PTurnDataStorage
{
    class PTurnDataFilesEnum : IEnumerable<PTurnData>
    {
        protected IEnumerable<byte[]> Chunks { get; }
        protected int ChunkSize { get; }
        protected ProcessedDataFileStructure PdfStructure { get; }
        public PTurnDataFilesEnum(
            IEnumerable<byte[]> chunks,
            int chunkSize,
            ProcessedDataFileStructure pdfStructure)
        {
            if (chunkSize < 1) throw new ArgumentException();
            Chunks = chunks;
            ChunkSize = chunkSize;
            PdfStructure = pdfStructure ?? throw new ArgumentNullException();
        }        

        public IEnumerator<PTurnData> GetEnumerator()
        {
            return new PTurnDataFilesEnumerator(Chunks.GetEnumerator(), PdfStructure);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new PTurnDataFilesEnumerator(Chunks.GetEnumerator(), PdfStructure);
        }
    }
}
