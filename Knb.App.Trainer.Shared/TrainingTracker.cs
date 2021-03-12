using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Trainer
{
    public class TrainingTracker
    {
        public TrainingTracker(TrainingStage trainingStage)
        {
            TrainingStage = trainingStage;
        }

        public TrainingStage TrainingStage { get; }
    }

    public enum TrainingStage { Init = 0, Training, Evaluating, Completed, Stopped }
}
