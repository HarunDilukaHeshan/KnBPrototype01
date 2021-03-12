using Knb.App.Simulator.GameData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.App.Simulator.Shared.Components
{
    public interface IDataRecorder
    {
        void Record(PlayData[] playDataArr);
        Task SaveAsync(string fileName, bool validateDataFile = false);
    }
}
