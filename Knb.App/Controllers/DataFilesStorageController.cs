using Knb.DataStorage.Shared.XmlStorage;
using Knb.DataStorage.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.App.Controllers
{
    public class DataFilesStorageController : ControllerBase
    {
        protected IXmlDataFilesStorage XmlDataFilesStorage { get; }
        public DataFilesStorageController(IXmlDataFilesStorage xmlDataFilesStorage)
        {
            XmlDataFilesStorage = xmlDataFilesStorage ?? throw new ArgumentNullException();
        }

        public async Task<bool> CreateDataFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException();
            return await XmlDataFilesStorage.CreateDataFileAsync(fileName);
        }

        public async Task<DataStorage.Storage.FileInfo[]> GetDataFilesInfoAsync()
        {
            return await XmlDataFilesStorage.GetFilesInfoAsync();
        }

        public bool Exists(string fileName)
        {
            return XmlDataFilesStorage.Exists(fileName);
        }
    }
}
