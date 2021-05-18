using System.Numerics;

namespace Polkadot.BinarySerializer.Types
{
    /// <summary>
    /// Warning: Not really an Uint128 in terms of arithmetics. Just a substitute for BinarySerializer to serialize/deserialize. 
    /// </summary>
    public struct UInt128
    {
        public BigInteger Value;

        public UInt128(ulong l0, ulong l1)
        {
            var val = new BigInteger(l1);
            val <<= 64;
            val |= l0;
            Value = val;
        }
        
        public static implicit operator BigInteger(UInt128 v)
        {
            return v.Value;
        }

        public static implicit operator UInt128(BigInteger v)
        {
            return new()
            {
                Value = v
            };
        }

        public void Deconstruct(out ulong l0, out ulong l1)
        {
            l0 = (ulong) (Value & 0xffff_ffff_ffff_fffful);
            l1 = (ulong) (Value >> 64);
        }
    }
}