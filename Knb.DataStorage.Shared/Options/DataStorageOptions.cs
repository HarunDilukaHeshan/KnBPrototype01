using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.DataStorage.Options
{
    public class DataStorageOptions
    {
        public static readonly string Position = "DataStorageOptions";
        public StorageSectionDetails DataFilesStorageSection { get; set; }
        public StorageSectionDetails ProcessedDataFilesStorageSection { get; set; }
        public StorageSectionDetails MLModelFiles { get; set; }
        public StorageSectionDetails TextFiles { get; set; }
    }
}
