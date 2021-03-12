using Knb.App.Trainer.Options;
using Knb.DataStorage.PTurnDataStorage;
using Knb.DataStorage.Shared.PTurnDataStorage;
using Knb.DataStorage.Storage;
using Microsoft.Extensions.Options;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Trainer.Components
{
    class SdcaMaximumEntropyTrainer : TrainerBase
    {
        public SdcaMaximumEntropyTrainer(
            IPTurnDataFilesStorage pTurnDataFilesStorage,
            IMLModelFilesStorage modelFilesStorage,
            IOptions<ProcessedDataFileStructure> pDataFileStructure)
            : base(pTurnDataFilesStorage, modelFilesStorage, pDataFileStructure)
        { }

        protected override IEstimator<ITransformer> AttachTrainingAlgorithm(IEstimator<ITransformer> pipeline)
        {
            return pipeline.Append(MLContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"));
        }
    }
}
