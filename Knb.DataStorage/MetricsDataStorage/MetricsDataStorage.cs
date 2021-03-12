using Knb.DataStorage.Storage;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.DataStorage.MetricsDataStorage
{
    class MetricsDataStorage : IMetricsDataStorage
    {
        protected ITextFilesStorage TextFilesStorage { get; }
        protected IMLModelFilesStorage ModelFilesStorage { get; }

        public MetricsDataStorage(
            ITextFilesStorage textFilesStorage, 
            IMLModelFilesStorage mLModelFilesStorage)            
        {
            TextFilesStorage = textFilesStorage ?? throw new ArgumentNullException();
            ModelFilesStorage = mLModelFilesStorage ?? throw new ArgumentNullException();
        }

        public async Task CreateMetricsFileAsync(string mlModelFileName, MulticlassClassificationMetrics metrics)
        {
            if (string.IsNullOrWhiteSpace(mlModelFileName)) throw new ArgumentException();
            _ = metrics ?? throw new ArgumentNullException();

            if (!ModelFilesStorage.Exists(mlModelFileName)) throw new InvalidOperationException();

            var str =
                "LogLoss: " + metrics.LogLoss + System.Environment.NewLine +
                "LogLossReduction: " + metrics.LogLossReduction + System.Environment.NewLine +
                "MacroAccuracy: " + metrics.MacroAccuracy + System.Environment.NewLine +
                "MicroAccuracy: " + metrics.MacroAccuracy + System.Environment.NewLine +
                "TopKAccuracy: " + metrics.TopKAccuracy + System.Environment.NewLine +
                "TopKPredictionCount: " + metrics.TopKPredictionCount + System.Environment.NewLine +
                "TopKAccuracyForAllK: " + metrics.TopKAccuracyForAllK + System.Environment.NewLine +
                "PerClassLogLoss: " + metrics.PerClassLogLoss + System.Environment.NewLine +
                "ConfusionMatrix: " + metrics.ConfusionMatrix + System.Environment.NewLine;

            await TextFilesStorage.CreateFileAsync(mlModelFileName, new byte[0]);
            using var st = TextFilesStorage.GetWriteStream(mlModelFileName);
            await st.WriteAsync(Encoding.UTF8.GetBytes(str));
            await st.DisposeAsync();
        }
    }
}
