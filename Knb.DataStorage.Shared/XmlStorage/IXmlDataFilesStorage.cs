using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Knb.DataStorage.Shared.XmlStorage
{
    public interface IXmlDataFilesStorage
    {
        Task<bool> CreateDataFileAsync(string fileName);
        IAsyncEnumerable<XmlDocument> GetAsyncChunks(string fileName);
        Task WriteXmlAsync(string fileName, string xmlStr);
        Task<bool> ValidateXmlFile(string fileName);
        bool Exists(string fileName);
        Task<Storage.FileInfo> GetFileInfoAsync(string fileName);
        Task<Storage.FileInfo[]> GetFilesInfoAsync();
    }
}
