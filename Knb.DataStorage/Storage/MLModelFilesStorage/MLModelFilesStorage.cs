using Knb.DataStorage.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Knb.DataStorage.Storage.MLModelFilesStorage
{
    class MLModelFilesStorage : DataStorageBase, IMLModelFilesStorage
    {
        public MLModelFilesStorage(
            StorageSectionDetails sectionDetails,
            FilesIoOptions filesIoOptions)
            : base(sectionDetails, filesIoOptions)
        {

        }

        public override IAsyncEnumerable<byte[]> GetAsyncChunks(string fileName)
        {
            throw new NotImplementedException();
        }

        public override async Task<bool> CreateFileAsync(string fileName, byte[] buffer)
        {
            fileName = string.IsNullOrWhiteSpace(fileName) ? fileName : Path.GetFileName(fileName);

            if (string.IsNullOrWhiteSpace(fileName) || !IsValidFileName(fileName)) 
                throw new ArgumentException();

            var datPath = GetPath(fileName) ?? throw new ArgumentException();

            if (File.Exists(datPath)) throw new InvalidOperationException("File already exists");

            if (!Directory.Exists(SectionDetails.Path.ToString()))
                Directory.CreateDirectory(SectionDetails.Path.ToString());

            using var writer = File.Create(datPath, FilesIoOptions.BufferSize);
            await writer.WriteAsync(buffer);
            await writer.FlushAsync();
            await writer.DisposeAsync();

            return File.Exists(datPath);
        }

        protected override string GetPath(string fileName)
        {
            var path = base.GetPath(fileName);
            return System.IO.Path.ChangeExtension(path, "ml");
        }
    }
}
