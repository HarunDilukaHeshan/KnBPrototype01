using Knb.DataStorage.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Knb.DataStorage.Storage
{
    public class DataFilesStorage : DataStorageBase, IDataFilesStorage
    {
        protected DataFileChunkOptions DataFileChunkOptions { get; }

        public DataFilesStorage(
            StorageSectionDetails sectionDetails,
            FilesIoOptions filesIoOptions,
            DataFileChunkOptions dataFileChunkOptions)
            : base(sectionDetails, filesIoOptions)
        {
            DataFileChunkOptions = dataFileChunkOptions;
        }

        public override IAsyncEnumerable<byte[]> GetAsyncChunks(string fileName)
        {            
            if (!IsValidFileName(fileName)) throw new ArgumentException();
            var datPath = GetPath(fileName);

            if (!File.Exists(datPath)) throw new FileNotFoundException();

            return new DataFilesAsyncEnum(datPath, DataFileChunkOptions.BufferSize);
        }
    }
}
