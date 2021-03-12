using Knb.App.Trainer.Options;
using Knb.App.Trainer.Shared;
using Knb.DataStorage.Options;
using Knb.DataStorage.PTurnDataStorage;
using Knb.DataStorage.Shared.PTurnDataStorage;
using Knb.DataStorage.Storage;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Knb.App.Trainer.Components
{
    public delegate void TrainingStageChanged(TrainingTracker trainingTracker);
    public abstract class TrainerBase
    {
        protected TrainerBaseOptions Options { get; set; }
        protected MLContext MLContext { get; }
        protected ProcessedDataFileStructure PDataFileStructure { get; }
        protected IPTurnDataFilesStorage PTurnDataFilesStorage { get; }
        protected IMLModelFilesStorage ModelFilesStorage { get; }
        private CancellationTokenSource TokenSource { get; }
        public TrainingStageChanged OnStageChanged { get; set; }
        public TrainerBase(
            IPTurnDataFilesStorage pTurnDataFilesStorage,
            IMLModelFilesStorage mLModelFilesStorage,
            IOptions<ProcessedDataFileStructure> pDataFileStructure)
        {
            PTurnDataFilesStorage = pTurnDataFilesStorage ?? throw new ArgumentNullException();
            ModelFilesStorage = mLModelFilesStorage ?? throw new ArgumentNullException();
            PDataFileStructure = pDataFileStructure.Value ?? throw new ArgumentNullException();
            MLContext = new MLContext();
            TokenSource = new CancellationTokenSource();
        }

        public void Stop()
        {
            TokenSource.Cancel();
        }

        public async Task<MulticlassClassificationMetrics> Start(Action<TrainerBaseOptions> action)
        {
            MulticlassClassificationMetrics metrics = null;
            var options = new TrainerBaseOptions();
            action.Invoke(options);
            Options = options;

            if (OnStageChanged != null) OnStageChanged.Invoke(new TrainingTracker(TrainingStage.Init));
            var trainingData = LoadTrainingData();
            var pipeline = PreprocessData();
            var token = TokenSource.Token;

            pipeline = AttachTrainingAlgorithm(pipeline);
            pipeline = LabelMapping(pipeline);

            await Task.Run(() =>
            {
                if (OnStageChanged != null) OnStageChanged.Invoke(new TrainingTracker(TrainingStage.Training));
                var trainedModel = BuildAndTrainModel(trainingData, pipeline);

                SaveMLModel(trainedModel, trainingData.Schema, Options.MLModelFileName);

                if (OnStageChanged != null) OnStageChanged.Invoke(new TrainingTracker(TrainingStage.Evaluating));
                metrics = EvaluateModel(trainedModel);

                if (OnStageChanged != null) OnStageChanged.Invoke(new TrainingTracker(TrainingStage.Completed));
            }, token);

            return metrics;
        }

        protected MulticlassClassificationMetrics EvaluateModel(ITransformer trainedModel)
        {
            var data = PTurnDataFilesStorage.GetChunks(Options.TestPDataFileName);
            var testData = MLContext.Data.LoadFromEnumerable<PTurnData>(data);

            var transformedData = trainedModel.Transform(testData);
            return MLContext.MulticlassClassification.Evaluate(transformedData);
        }

        protected void SaveMLModel(ITransformer mlModel, DataViewSchema trainingDataViewSchema, string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException();

            if (!ModelFilesStorage.Exists(path)) ModelFilesStorage.CreateFileAsync(path, new byte[0]);

            var st = ModelFilesStorage.GetWriteStream(path);
            MLContext.Model.Save(mlModel, trainingDataViewSchema, st);
        }

        protected virtual IDataView LoadTrainingData()
        {
            var cEnum = PTurnDataFilesStorage.GetChunks(Options.ProcessedDataFileName);
            var trainingData = MLContext.Data.LoadFromEnumerable<PTurnData>(cEnum);

            return trainingData;
        }

        protected virtual IEstimator<ITransformer> PreprocessData()
        {
            var pipe = MLContext.Transforms.Conversion.MapValueToKey(inputColumnName: "FacedUpCards", outputColumnName: "Label")
                .Append(MLContext.Transforms.Text.FeaturizeText(inputColumnName: "Hand", outputColumnName: "FeaturizedHand"))
                .Append(MLContext.Transforms.Text.FeaturizeText(inputColumnName: "PrevTurns", outputColumnName: "FeaturizedPrevTurns"))
                .Append(MLContext.Transforms.Text.FeaturizeText(inputColumnName: "ActiveCards", outputColumnName: "FeaturizedActiveCards"))
                .Append(MLContext.Transforms.Text.FeaturizeText(inputColumnName: "InactiveCards", outputColumnName: "FeaturizedInactiveCards"))
                .Append(MLContext.Transforms.Concatenate("Features", "FeaturizedHand", "FeaturizedPrevTurns", "FeaturizedActiveCards", "FeaturizedInactiveCards"));

            return pipe;
        }

        protected abstract IEstimator<ITransformer> AttachTrainingAlgorithm(IEstimator<ITransformer> pipeline);

        protected virtual IEstimator<ITransformer> LabelMapping(IEstimator<ITransformer> pipeline)
        {
            return pipeline.Append(MLContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
        }

        protected virtual ITransformer BuildAndTrainModel(IDataView trainingData, IEstimator<ITransformer> pipeline)
        {
            return pipeline.Fit(trainingData);
        }
    }
}
