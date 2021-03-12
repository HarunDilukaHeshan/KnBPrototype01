using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Trainer.Shared
{
    public class PreprocessorTracker
    {
        public PreprocessorTracker(long completed, long taskSize)
        {
            if (taskSize < completed) throw new ArgumentException();
            TaskSize = taskSize;
            Completed = completed;
        }
        public long TaskSize { get; }
        public long Completed { get; }
    }
}
