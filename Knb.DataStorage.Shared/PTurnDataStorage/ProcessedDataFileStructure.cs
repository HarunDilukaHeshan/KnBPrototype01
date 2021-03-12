using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.DataStorage.Shared.PTurnDataStorage
{
    public class ProcessedDataFileStructure
    {
        public static readonly string Position = "ProcessedDataFileStructure";
        public string[] Columns { get; set; }
    }
}
