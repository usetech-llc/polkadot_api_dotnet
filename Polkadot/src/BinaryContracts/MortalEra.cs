using System;
using System.IO;
using Polkadot.Utils;

namespace Polkadot.BinaryContracts
{
    public class MortalEra
    {
        public ulong Period;
        public ulong Phase;

        public MortalEra()
        {
        }

        public MortalEra(ulong period, ulong phase)
        {
            Period = period;
            Phase = phase;
        }
        
        public static MortalEra FromCurrentBlock(ulong? period, ulong block) {
            period = Math.Min(Math.Max(period?.NextPowerOfTwo() ?? 1UL << 16, 4), 1UL << 16); 

            var phase = block % period.Value;
            var quantizeFactor = Math.Max(period.Value >> 12, 1UL);
            var quantizedPhase = phase / quantizeFactor * quantizeFactor;

            return new MortalEra(period.Value, quantizedPhase);
        }
    }
}