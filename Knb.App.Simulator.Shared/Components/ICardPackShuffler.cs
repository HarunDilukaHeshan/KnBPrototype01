using Knb.App.Simulator.GameData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Simulator.Components
{
    public interface ICardPackShuffler 
    { 
        void Shuffle(CardPack[] cardPack); 
    }
}
