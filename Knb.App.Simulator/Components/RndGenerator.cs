using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Knb.App.Simulator.Components
{
    public class RndGenerator : IRndGenerator, IDisposable
    {
        protected RNGCryptoServiceProvider RngCsp { get; } = new RNGCryptoServiceProvider();

        public int Next(int minValue, int maxValue)
        {
            if (minValue == maxValue || minValue > maxValue) throw new ArgumentException();

            var bytes = new byte[4];
            RngCsp.GetBytes(bytes);
            var val = BitConverter.ToInt32(bytes);

            var tempVal = (val % (maxValue - minValue));

            return (tempVal < 0) ? (tempVal * -1) + minValue : tempVal + minValue;
        }

        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }

        public void Dispose()
        {
            RngCsp.Dispose();
        }

        ~RndGenerator()
        {
            RngCsp.Dispose();
        }
    }
}
