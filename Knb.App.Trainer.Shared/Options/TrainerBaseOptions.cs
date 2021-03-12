using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Trainer.Options
{
    public class TrainerBaseOptions
    {
        public string ProcessedDataFileName { get; set; }
        public string MLModelFileName { get; set; }
        public string TestPDataFileName { get; set; }
    }
}
