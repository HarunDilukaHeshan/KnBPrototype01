using Knb.DataStorage.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.DataStorage.Storage.TextFilesStorage
{
    class TextFilesStorage : DataStorageBase, ITextFilesStorage
    {
        public TextFilesStorage(
            StorageSectionDetails sectionDetails,
            FilesIoOptions filesIoOptions)
            : base(sectionDetails, filesIoOptions)
        { }

        public override IAsyncEnumerable<byte[]> GetAsyncChunks(string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task WriteAsync(string fileName, string text)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(text))
                throw new ArgumentException();

            if (!Exists(fileName) && !(await CreateFileAsync(fileName, new byte[0])))
                throw new InvalidOperationException();

            using var st = GetWriteStream(fileName);
            await st.WriteAsync(Encoding.UTF8.GetBytes(text));
            await st.DisposeAsync();
        }

        protected override string GetPath(string fileName)
        {
            var path = base.GetPath(fileName);
            return System.IO.Path.ChangeExtension(path, "txt");
        }
    }
}
