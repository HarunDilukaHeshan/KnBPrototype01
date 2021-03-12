using Knb.DataStorage.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.App.Controllers
{
    public class ModelFilesStorageController : ControllerBase
    {
        protected IMLModelFilesStorage ModelFilesStorage { get; }
        public ModelFilesStorageController(IMLModelFilesStorage modelFilesStorage)
        {
            ModelFilesStorage = modelFilesStorage ?? throw new ArgumentNullException();
        }

        public async Task<DataStorage.Storage.FileInfo[]> GetModelFilesInfoAsync()
        {
            return await ModelFilesStorage.GetFilesInfo();
        }

        public bool Exists(string fileName)
        {
            return ModelFilesStorage.Exists(fileName);
        }
    }
}
