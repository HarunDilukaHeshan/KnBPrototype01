using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.DataStorage.Storage
{
    public class PTurnData
    {
        [LoadColumn(0)]
        public string FacedUpCards { get; set; }
        [LoadColumn(1)]
        public string Hand { get; set; }
        [LoadColumn(2)]
        public string PrevTurns { get; set; }
        [LoadColumn(3)]
        public string ActiveCards { get; set; }
        [LoadColumn(4)]
        public string InactiveCards { get; set; }
    }
}
