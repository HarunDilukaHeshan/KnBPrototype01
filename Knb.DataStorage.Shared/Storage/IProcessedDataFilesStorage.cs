using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Knb.DataStorage.Storage
{
    public interface IProcessedDataFilesStorage : IDataStorage
    {
        IEnumerable<byte[]> GetChunks(string fileName);
    }
}
