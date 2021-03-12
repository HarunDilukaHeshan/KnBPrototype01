using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Trainer
{
    public interface IDataPreprocessor
    {
        Task<TurnData[]> ProcessAsync(PlayData[] playDataArray);
    }

    public class DataPreprocessor : IDataPreprocessor
    {
        public async Task<TurnData[]> ProcessAsync(PlayData[] playDataArray)
        {
            if (playDataArray == null) throw new ArgumentNullException();
            var selectedTurnArr = new TurnData[0];

            await Task.Run(() => { selectedTurnArr = Filter(playDataArray); });            

            return selectedTurnArr;
        }

        private TurnData[] Filter(PlayData[] playDataArray)
        {
            if (playDataArray.Length < 1) throw new InvalidOperationException();
            var selectedTurnList = new List<TurnData>();

            foreach (var playData in playDataArray)
            {
                var roundDataArr = playData.RoundData;
                var firstRound = roundDataArr.FirstOrDefault() ?? throw new InvalidOperationException();
                var players = (from td in firstRound.TurnDataArray orderby td.PlayerId select td.PlayerId)
                    .Distinct()
                    .ToArray();
                var winnerId = "";                
                
                foreach(var rd in roundDataArr)
                {
                    bool idFound = false;
                    foreach (var playerId in players)
                    {                        
                        foreach (var td in rd.TurnDataArray)
                        {
                            idFound = playerId == td.PlayerId;
                            if (idFound) break;
                        }

                        if (!idFound)
                        {
                            winnerId = playerId;
                            break;
                        }
                    }

                    if (!idFound) break;
                }

                if (string.IsNullOrWhiteSpace(winnerId)) throw new InvalidOperationException();

                foreach (var rd in roundDataArr)                
                    foreach (var td in rd.TurnDataArray)
                        if (td.PlayerId == winnerId)
                            selectedTurnList.Add(td);                
            }

            return selectedTurnList.ToArray();
        }
    }

}
