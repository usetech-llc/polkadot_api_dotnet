using System;
using System.Collections.Generic;
using System.Linq;

namespace Polkadot.BinarySerializer
{
    public class ContractSelectorComparer : IEqualityComparer<(byte[] destPublicKey, byte[] selector)>
    {
        public bool Equals((byte[] destPublicKey, byte[] selector) x, (byte[] destPublicKey, byte[] selector) y)
        {
            var minLength = Math.Min(x.selector.Length, y.selector.Length);
            return x.destPublicKey.SequenceEqual(y.destPublicKey)
                   && x.selector.AsSpan(0, minLength).SequenceEqual(y.selector.AsSpan(0, minLength));
        }

        public int GetHashCode((byte[] destPublicKey, byte[] selector) obj)
        {
            return obj.Item1[0] ^ obj.Item2[0];
        }
    }
}