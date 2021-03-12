using Knb.App.Simulator.GameData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.App.Simulator.Components
{
    public interface IPlayManager
    {
        CardPack[] CardPacks { get; set; }
        Task<PlayData> PlayAsync(Players players);
    }
}
