using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.DataStorage.Options
{
    public class ProcessedDataFileChunkOptions
    {
        public static readonly string Position = "ProcessedDataFileChunkOptions";
        public int BufferSize { get; set; }
    }
}
