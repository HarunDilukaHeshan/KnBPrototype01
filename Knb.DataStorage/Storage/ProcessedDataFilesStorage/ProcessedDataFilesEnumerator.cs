using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Knb.DataStorage.Storage
{
    class ProcessedDataFilesEnumerator : IEnumerator<byte[]>
    {
        private byte[] _current;
        public byte[] Current { get { return _current.ToArray(); } }

        object IEnumerator.Current { get { return _current.ToArray(); } }
        protected FileStream Reader { get; }
        protected int ChunkSize { get; }

        public ProcessedDataFilesEnumerator(FileStream reader, int chunkSize)
        {
            Reader = reader;
            ChunkSize = chunkSize;
        }

        public void Dispose()
        {
            Reader.Dispose();
        }

        public bool MoveNext()
        {
            var len = (Reader.Length < ChunkSize) ? Convert.ToInt32(Reader.Length) : ChunkSize;
            var chunks = new byte[len];
            var result = Reader.Length > Reader.Position;

            Reader.Read(chunks, 0, len);
            _current = chunks;

            return result;
        }

        public void Reset()
        {
            Reader.Position = -1;
        }
    }
}
