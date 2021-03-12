using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Trainer.Options
{
    public class TrainerOptions
    {
        /// <summary>
        /// Processed data file name
        /// </summary>
        public string ProcessedDataFileName { get; set; }
        /// <summary>
        /// ML model file name
        /// </summary>
        public string MLModelFileName { get; set; }
        /// <summary>
        /// Data file name
        /// </summary>
        public string DataFileName { get; set; }
        /// <summary>
        /// Training data file
        /// </summary>
        public string TestDataFileName { get; set; }
    }
}
