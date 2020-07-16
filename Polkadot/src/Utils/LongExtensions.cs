using System.Collections.Generic;

namespace Polkadot.Utils
{
    public static class LongExtensions
    {
        private static readonly Dictionary<ulong, int> TrailingZeroesCache;

        static LongExtensions()
        {
            TrailingZeroesCache = new Dictionary<ulong, int>();

            for (int i = 0; i < 64; i++)
            {
                TrailingZeroesCache[1ul << i] = i;
            }

            TrailingZeroesCache[0] = 64;
        }

        public static int TrailingZeroes(this ulong value)
        {
            var clearAllButFirstOneBit = value & (ulong)-(long)value;
            return TrailingZeroesCache[clearAllButFirstOneBit];
        }
    }
}