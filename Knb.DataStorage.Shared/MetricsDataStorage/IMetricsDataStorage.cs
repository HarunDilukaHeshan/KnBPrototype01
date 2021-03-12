using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.DataStorage.Storage
{
    public interface IMetricsDataStorage
    {
        Task CreateMetricsFileAsync(string mlModelFileName, MulticlassClassificationMetrics metrics);
    }
}
