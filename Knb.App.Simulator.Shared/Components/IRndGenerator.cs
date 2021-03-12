using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.App.Simulator.Components
{
    public interface IRndGenerator
    {
        int Next(int minValue, int maxValue);
        int Next(int maxValue);
    }
}
