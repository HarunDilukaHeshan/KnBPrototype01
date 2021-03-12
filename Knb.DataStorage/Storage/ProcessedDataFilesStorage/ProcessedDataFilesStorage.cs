using Knb.DataStorage.Options;
using Knb.DataStorage.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Knb.DataStorage.Storage
{
    class ProcessedDataFilesStorage : DataStorageBase, IProcessedDataFilesStorage
    {
        protected ProcessedDataFileChunkOptions ProcessedDataFileChunkOptions { get; }

        public ProcessedDataFilesStorage(
            StorageSectionDetails sectionDetails,
            FilesIoOptions filesIoOptions,
            ProcessedDataFileChunkOptions processedDataFileChunkOptions)
            : base(sectionDetails, filesIoOptions)
        {
            ProcessedDataFileChunkOptions = processedDataFileChunkOptions;
        }

        public override IAsyncEnumerable<byte[]> GetAsyncChunks(string fileName)
        {
            if (!IsValidFileName(fileName)) throw new ArgumentException();
            var datPath = GetPath(fileName);

            if (!File.Exists(datPath)) throw new FileNotFoundException();

            return new ProcessedDataFilesAsyncEnum(datPath, ProcessedDataFileChunkOptions.BufferSize);
        }

        public IEnumerable<byte[]> GetChunks(string fileName)
        {
            if (!IsValidFileName(fileName)) throw new ArgumentException();
            var datPath = GetPath(fileName);

            if (!File.Exists(datPath)) throw new FileNotFoundException();

            return new ProcessedDataFilesEnum(datPath, ProcessedDataFileChunkOptions.BufferSize);
        }
    }
}
