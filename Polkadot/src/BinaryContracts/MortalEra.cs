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
    }
}