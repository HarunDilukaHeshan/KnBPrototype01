using Knb.DataStorage.PTurnDataStorage;
using Knb.DataStorage.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.App.Controllers
{
    public class PDataFilesStorageController : ControllerBase
    {
        protected IPTurnDataFilesStorage PDataFilesStorage { get; }
        public PDataFilesStorageController(
            IPTurnDataFilesStorage pDataFilesStorage)
        {
            PDataFilesStorage = pDataFilesStorage ?? throw new ArgumentNullException();
        }

        public async Task<Knb.DataStorage.Storage.FileInfo[]> GetFilesInfoAsync()
        {
            return await PDataFilesStorage.GetFilesInfoAsync();
        }

        public async Task<bool> CreatePDataFile(string fileName)
        {
            return await PDataFilesStorage.CreateDataFileAsync(fileName);
        }

        public bool Exists(string fileName)
        {
            return PDataFilesStorage.Exists(fileName);
        }
    }
}
