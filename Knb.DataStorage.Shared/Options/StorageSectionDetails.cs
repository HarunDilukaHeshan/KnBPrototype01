using Knb.DataStorage.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.DataStorage.Options
{
    public class StorageSectionDetails
    {
        public Uri Path { get; set; }
        public StorageSections StorageSection { get; set; }
    }
}
