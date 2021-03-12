using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.DataStorage.Storage
{
    public class FileInfo
    {
        public FileInfo(string fileName, string extension, string directory, long length)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(extension))
                throw new ArgumentException();

            FileName = fileName ?? throw new ArgumentNullException();
            Extension = extension ?? throw new ArgumentNullException();
            DirectoryName = directory ?? throw new ArgumentNullException();
            Length = length;
        }

        public string DirectoryName { get; }
        public long Length { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
}
