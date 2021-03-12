using Knb.App.Trainer.Components;
using Knb.App.Trainer.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.App.Controllers
{
    public class TrainerController : ControllerBase
    {
        protected IKnbTrainer Trainer { get; }
        public TrainerPhaseChangedEvent OnPhaseChangedEvent { get; set; }
        public PreprocessorChanged OnPreprocessorChanged { get; set; } 

        public TrainerController(IKnbTrainer trainer)
        {
            Trainer = trainer;            
        }

        public async Task<Microsoft.ML.Data.MulticlassClassificationMetrics> Start(Action<TrainerOptions> action)
        {
            if (action == null) throw new ArgumentNullException();
            var options = new TrainerOptions();
            action.Invoke(options);

            Trainer.OnPreprocessorChanged = OnPreprocessorChanged;
            Trainer.OnPhaseChangedEvent = OnPhaseChangedEvent;

            var metrics = await Trainer.Start(op => {
                op.DataFileName = options.DataFileName;
                op.TestDataFileName = options.TestDataFileName;
                op.ProcessedDataFileName = options.ProcessedDataFileName;
                op.MLModelFileName = options.MLModelFileName;
            });

            return metrics;
        }

        public void Stop()
        {
            Trainer.Stop();
        }
    }
}
