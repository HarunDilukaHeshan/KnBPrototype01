using Knb.App.Trainer.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.App.Trainer.Components
{
    public delegate void TrainerPhaseChangedEvent(TrainerPhases phase);
    public enum TrainerPhases { DataPreprocessing, TestDataPreprocessing, Training, Testing, Completed, Stopped }
    public interface IKnbTrainer
    {
        TrainerPhaseChangedEvent OnPhaseChangedEvent { get; set; }
        PreprocessorChanged OnPreprocessorChanged { get; set; }
        Task<Microsoft.ML.Data.MulticlassClassificationMetrics> Start(Action<TrainerOptions> action);
        void Stop();
    }
}
