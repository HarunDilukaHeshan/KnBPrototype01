using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.DataStorage.Options
{
    public class DataFileChunkOptions
    {
        public static readonly string Position = "DataFileChunkOptions";
        public int BufferSize { get; set; }
    }
}
