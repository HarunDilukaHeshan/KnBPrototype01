using Knb.App.Trainer.Options;
using Knb.DataStorage.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.App.Trainer.Components
{
    class KnbTrainer : IKnbTrainer
    {
        protected IDataPreprocessor DataPreprocessor { get; }
        protected TrainerBase Trainer { get; }
        public bool HasStarted { get; protected set; } = false;
        protected PreprocessorStopped OnPreprocessorStop { get; set; }        
        protected TrainingStageChanged OnStageChanged { get; set; }
        protected IMetricsDataStorage MetricsDataStorage { get; }
        public TrainerPhaseChangedEvent OnPhaseChangedEvent { get; set; }
        public PreprocessorChanged OnPreprocessorChanged { get; set; }        
        public KnbTrainer(
            IDataPreprocessor dataPreprocessor,
            TrainerBase trainerBase, IMetricsDataStorage metricsDataStorage)
        {
            DataPreprocessor = dataPreprocessor ?? throw new ArgumentNullException();
            Trainer = trainerBase ?? throw new ArgumentNullException();
            MetricsDataStorage = metricsDataStorage ?? throw new ArgumentNullException();
        }

        public async Task<Microsoft.ML.Data.MulticlassClassificationMetrics> Start(Action<TrainerOptions> action)
        {
            _ = action ?? throw new ArgumentNullException();
            var options = new TrainerOptions();
            action.Invoke(options);

            Microsoft.ML.Data.MulticlassClassificationMetrics metrics = null;

            try
            {
                HasStarted = true;

                DataPreprocessor.OnPreprocessorChanged = OnPreprocessorChanged ?? DataPreprocessor.OnPreprocessorChanged;
                DataPreprocessor.OnPreprocessorStop = OnPreprocessorStop ?? DataPreprocessor.OnPreprocessorStop;
                Trainer.OnStageChanged = (tracker) =>
                {
                    if (tracker.TrainingStage == TrainingStage.Evaluating)
                        OnPhaseChangedEvent?.Invoke(TrainerPhases.Testing);
                };

                OnPhaseChangedEvent?.Invoke(TrainerPhases.DataPreprocessing);
                await DataPreprocessor.Start(o =>
                {
                    o.DataFileName = options.DataFileName;
                    o.PDataFileName = options.ProcessedDataFileName;
                });

                OnPhaseChangedEvent?.Invoke(TrainerPhases.TestDataPreprocessing);
                await DataPreprocessor.Start(o =>
                {
                    o.DataFileName = options.TestDataFileName;
                    o.PDataFileName = "Processed" + options.TestDataFileName;
                });

                OnPhaseChangedEvent?.Invoke(TrainerPhases.Training);
                metrics = await Trainer.Start(o =>
                {
                    o.ProcessedDataFileName = options.ProcessedDataFileName;
                    o.TestPDataFileName = "Processed" + options.TestDataFileName;
                    o.MLModelFileName = options.MLModelFileName;
                });

                await MetricsDataStorage.CreateMetricsFileAsync(options.MLModelFileName, metrics);

                OnPhaseChangedEvent?.Invoke(TrainerPhases.Completed);                
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("User may not have permission to access the required file");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                throw new System.IO.FileNotFoundException("File does not found");
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured");
            }
            finally
            {
                HasStarted = false;
            }

            return metrics;
        }

        public void Stop()
        {
            try
            {
                DataPreprocessor.Stop();
                Trainer.Stop();
                OnPhaseChangedEvent?.Invoke(TrainerPhases.Stopped);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException("User may not have permission to access the required file");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                throw new System.IO.FileNotFoundException("File does not found");
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occured");
            }
            finally
            {
                HasStarted = false;
            }
        }
    }
}
