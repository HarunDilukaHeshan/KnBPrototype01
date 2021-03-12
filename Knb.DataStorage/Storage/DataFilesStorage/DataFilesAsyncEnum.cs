using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Knb.DataStorage.Storage
{
    class DataFilesAsyncEnum : IAsyncEnumerable<byte[]>
    {
        protected string FileUri { get; }
        protected int ChunkSize { get; }
        public DataFilesAsyncEnum(string fileUri, int chunkSize)
        {
            if (string.IsNullOrWhiteSpace(fileUri) || chunkSize < 1) throw new ArgumentException();
            FileUri = fileUri;
            ChunkSize = chunkSize;
        }

        public IAsyncEnumerator<byte[]> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var reader = File.Open(FileUri, FileMode.Open, FileAccess.Read, FileShare.Read);

            return new DataFilesAsyncEnumerator(reader, ChunkSize);
        }
    }
}
