using Knb.DataStorage.Shared.PTurnDataStorage;
using Knb.DataStorage.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knb.DataStorage.PTurnDataStorage
{
    class PTurnDataFilesEnumerator : IEnumerator<PTurnData>
    {
        public PTurnData Current { get; protected set; }
        protected IList<PTurnData> LinesList { get; } = new List<PTurnData>();
        protected string NewLine { get; set; }
        protected string LastStr { get; set; }
        protected ProcessedDataFileStructure PdfStructure { get; }
        protected IEnumerator<byte[]> ChunksEnumerator { get; }
        protected int ChunkSize { get; }
        protected int MaxRetryCount { get; } = 100;

        object IEnumerator.Current { get { return Current; } }

        PTurnData IEnumerator<PTurnData>.Current { get { return Current; } }

        public PTurnDataFilesEnumerator(
                IEnumerator<byte[]> chunksEnumerator,
                ProcessedDataFileStructure pdfStructure)
        {
            ChunksEnumerator = chunksEnumerator ?? throw new ArgumentNullException();
            PdfStructure = pdfStructure ?? throw new ArgumentNullException();
        }                

        public void Dispose()
        {
            ChunksEnumerator.Dispose();
        }

        public bool MoveNext()
        {
            var result = true;
            
            if (LinesList.Count > 0)
            {
                Current = LinesList[0];
                LinesList.RemoveAt(0);
            }
            else
            {
                int i;
                for (i = 0; i < MaxRetryCount; i++)
                {
                    result = ChunksEnumerator.MoveNext();
                    if (result)
                    {
                        var c = ChunksEnumerator.Current;
                        var str = LastStr + Encoding.UTF8.GetString(c);
                        if (string.IsNullOrEmpty(NewLine)) NewLine = GetEmbeddedNewLineChars(str);
                        var lines = (string.IsNullOrEmpty(NewLine)) ? new string[0] : str.Split(NewLine);

                        if (Current == null && lines.Length > 0)
                            if (!ValidateHeader(lines[0])) throw new InvalidOperationException();

                        if (Current == null && lines.Length == 2)
                        {
                            LastStr = lines[1];
                            continue;
                        }
                        else if (Current == null && lines.Length > 2)
                        {
                            for (int t = 1; t < lines.Length - 1; t++)
                                LinesList.Add(GetPTurnData(lines[t]));
                            Current = LinesList[0];
                            LinesList.RemoveAt(0);
                            LastStr = lines[^1];
                            break;
                        }
                        else if (lines.Length > 1)
                        {
                            for (int t = 1; t < lines.Length - 1; t++)
                                LinesList.Add(GetPTurnData(lines[t]));
                            Current = GetPTurnData(lines[0]);
                            LastStr = lines[^1];
                            break;
                        }
                        else if (lines.Length == 1)
                        {
                            LastStr = str;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(LastStr))
                    {
                        var lines = LastStr.Split(NewLine);
                        if (lines.Length > 0)
                            Current = GetPTurnData(lines[0]);
                        LastStr = "";
                        result = true;
                        break;
                    }
                    else
                        break;
                }

                if (i >= MaxRetryCount - 1) throw new InvalidOperationException();
            }
            if (result == false)
            {

            }
            return result;
        }

        public void Reset()
        {
            Current = null;
            LastStr = "";
            NewLine = "";
            ChunksEnumerator.Reset();
        }

        private string GetEmbeddedNewLineChars(string value)
        {
            char r = '\r', n = '\n';

            var rnI = value.IndexOf(string.Concat(r, n));
            var rI = value.IndexOf(r);
            var nI = value.IndexOf(n);

            if (rnI > -1)
                return string.Concat(r, n);
            if (r > -1 && value.Length - 1 != rI)
                return r.ToString();
            if (n > -1 && value.Length - 1 != nI)
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
        private PTurnData GetPTurnData(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) throw new ArgumentException();
            try
            {
                if (line.Count(c => c == '\t') != 4) throw new InvalidOperationException("Invalid format");
            }
            catch(Exception ex)
            {

            }
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
    }
}
