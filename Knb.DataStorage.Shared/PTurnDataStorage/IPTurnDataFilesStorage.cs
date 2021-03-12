using Knb.DataStorage.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.DataStorage.PTurnDataStorage
{
    public interface IPTurnDataFilesStorage
    {
        Task<bool> CreateDataFileAsync(string fileName);
        IAsyncEnumerable<PTurnData[]> GetAsyncChunks(string fileName);
        IEnumerable<PTurnData> GetChunks(string fileName);
        Task WriteAsync(string fileName, PTurnData[] pTurnDataArr);
        Task<Storage.FileInfo[]> GetFilesInfoAsync();
        bool Exists(string fileName);
    }
}
