using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Knb.DataStorage.Storage
{
    public interface IDataStorage
    {
        Task<bool> CreateFileAsync(string fileName, byte[] buffer);
        Task<bool> DeleteFileAsync(string fileName);
        Task ClearFilesAsync();
        FileStream GetReadStream(string fileName);
        FileStream GetWriteStream(string fileName);
        Task<FileInfo[]> GetFilesInfo();
        Task<FileInfo> GetFileInfo(string fileName);
        bool Exists(string fileName);
        IAsyncEnumerable<byte[]> GetAsyncChunks(string fileName);
    }
}
