using Knb.DataStorage.Shared.PTurnDataStorage;
using Knb.DataStorage.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knb.DataStorage.PTurnDataStorage
{
    class PTurnDataFilesAsyncEnumerator : IAsyncEnumerator<PTurnData[]>
    {
        protected PTurnData[] CurrentArr { get; set; }
        protected string TheLastLine { get; set; } = "";
        protected string NewLine { get; set; }
        protected ProcessedDataFileStructure PdfStructure { get; }
        protected IAsyncEnumerator<byte[]> ChunksEnumerator { get; }
        protected int ChunkSize { get; }
        public PTurnData[] Current { get { return CurrentArr.ToArray(); } }        
        public PTurnDataFilesAsyncEnumerator(
            IAsyncEnumerator<byte[]> chunksEnumerator, 
            int chunkSize, 
            ProcessedDataFileStructure pdfStructure)
        {
            if (chunkSize < 1) throw new ArgumentException();
            ChunksEnumerator = chunksEnumerator ?? throw new ArgumentNullException();
            ChunkSize = chunkSize;
            PdfStructure = pdfStructure ?? throw new ArgumentNullException();
        }

        public async ValueTask DisposeAsync()
        {
            await ChunksEnumerator.DisposeAsync();
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            var result = false;
            var ptdList = new List<PTurnData>();

            result = await ChunksEnumerator.MoveNextAsync();
            while (result && ptdList.Count < ChunkSize)
            {
                var c = ChunksEnumerator.Current;
                var cStr = TheLastLine + Encoding.UTF8.GetString(c);
                NewLine = GetEmbeddedNewLineChars(cStr) ?? throw new InvalidOperationException("Invalid format");
                var lines = cStr.Split(NewLine);

                if (CurrentArr == null && (lines.Length == 0 || !ValidateHeader(lines[0])))
                    throw new InvalidOperationException("Invalid format");
                if (lines.Length == 1) { result = false; break; }

                for (int i = 0; i < lines.Length - 1; i++)
                {
                    if (CurrentArr == null && i == 0) continue;

                    ptdList.Add(GetPTurnData(lines[i]));
                }

                result = await ChunksEnumerator.MoveNextAsync();
                if (result) TheLastLine = lines[^1]; else ptdList.Add(GetPTurnData(lines[^1]));
            }

            CurrentArr = ptdList.ToArray();

            return result;
        }

        private PTurnData GetPTurnData(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) throw new ArgumentException();
            if (line.Count(c => c == '\t') != 4) throw new InvalidOperationException("Invalid format");

            var cols = line.Split('\t');

            return new PTurnData
            {
                FacedUpCards = cols[0],
                Hand = cols[1],
                PrevTurns = cols[2],
                ActiveCards = cols[3],
                InactiveCards = cols[4]
            };
        }

        private string GetEmbeddedNewLineChars(string value)
        {
            char r = '\r', n = '\n';

            if (value.Contains(string.Concat(r, n)))
                return string.Concat(r, n);
            if (value.Contains(r))
                return r.ToString();
            if (value.Contains(n))
                return n.ToString();
            else
                return null;
        }

        private bool ValidateHeader(string headerStr)
        {
            var hColumns1 = headerStr.Split('\t');
            var hColumns2 = PdfStructure.Columns;

            var result = (hColumns1.Length == hColumns2.Length);

            for (int i = 0; i < hColumns2.Length; i++)
                result = result && hColumns1[i] == hColumns2[i];

            return result;
        }
    }
}
