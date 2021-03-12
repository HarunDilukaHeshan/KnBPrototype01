using Knb.App.Trainer.Options;
using Knb.App.Trainer.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.App.Trainer.Components
{
    public interface IDataPreprocessor
    {
        Task Start(Action<DataPreprocessorOptions> action);
        void Stop();
        PreprocessorStopped OnPreprocessorStop { get; set; }
        PreprocessorChanged OnPreprocessorChanged { get; set; }
    }

    public delegate void PreprocessorStopped(PreprocessorTracker tracker);
    public delegate void PreprocessorChanged(PreprocessorTracker tracker);
}
