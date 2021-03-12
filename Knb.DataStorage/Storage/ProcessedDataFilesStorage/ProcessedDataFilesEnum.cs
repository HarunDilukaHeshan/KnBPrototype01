using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Knb.DataStorage.Storage
{
    class ProcessedDataFilesEnum : IEnumerable<byte[]>
    {
        protected string FileUri { get; }
        protected int ChunkSize { get; }

        public ProcessedDataFilesEnum(string fileUri, int chunkSize)
        {
            if (string.IsNullOrWhiteSpace(fileUri) || chunkSize < 1) throw new ArgumentException();
            FileUri = fileUri;
            ChunkSize = chunkSize;
        }

        public IEnumerator<byte[]> GetEnumerator()
        {
            var reader = File.Open(FileUri, FileMode.Open, FileAccess.Read, FileShare.Read);
            return new ProcessedDataFilesEnumerator(reader, ChunkSize);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
