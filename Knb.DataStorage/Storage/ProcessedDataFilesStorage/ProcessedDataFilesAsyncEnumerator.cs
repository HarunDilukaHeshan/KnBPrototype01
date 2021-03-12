using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knb.DataStorage.Storage
{
    public class ProcessedDataFilesAsyncEnumerator : IAsyncEnumerator<byte[]>
    {
        private byte[] _current = new byte[0];

        public byte[] Current { get { return _current.ToArray(); } }

        protected FileStream Reader { get; }
        protected int ChunkSize { get; }

        public ProcessedDataFilesAsyncEnumerator(FileStream reader, int chunkSize)
        {
            Reader = reader;
            ChunkSize = chunkSize;
        }

        public async ValueTask DisposeAsync()
        {
            await Task.Run(() => { Reader.Dispose(); });
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            var len = (Reader.Length < ChunkSize) ? Convert.ToInt32(Reader.Length) : ChunkSize;
            var chunks = new byte[len];
            var result = Reader.Length > Reader.Position;


            await Reader.ReadAsync(chunks, 0, len);
            
            _current = chunks;
            return result;
        }
    }
}
