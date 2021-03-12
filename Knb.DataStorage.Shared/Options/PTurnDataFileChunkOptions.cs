using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.DataStorage.Options
{
    public class PTurnDataFileChunkOptions
    {
        public static readonly string Position = "PTurnDataFileChunkOptions";
        public int BufferSize { get; set; }
    }
}
