using System.Numerics;

namespace Polkadot.BinarySerializer.Types
{
    /// <summary>
    /// Warning: Not really an Int128 in terms of arithmetics. Just a substitute for BinarySerializer to serialize/deserialize. 
    /// </summary>
    public struct Int128
    {
        public BigInteger Value;

        public Int128(ulong l0, ulong l1)
        {
            var negative = (l1 & 0x8000_0000_0000_0000ul) == 0x8000_0000_0000_0000ul;
            if (negative)
            {
                l0--;
                l0 = ~l0;
                l1 = ~l1;
            }

            var val = new BigInteger(l1);
            val <<= 64;
            val |= l0;
            Value = negative ? -val : val;
        }
        
        public static implicit operator BigInteger(Int128 v)
        {
            return v.Value;
        }

        public static implicit operator Int128(BigInteger v)
        {
            return new()
            {
                Value = v
            };
        }

        public void Deconstruct(out ulong l0, out ulong l1)
        {
            var negative = Value < 0;
            var value = negative ? -Value : Value;
            
            
            l0 = (ulong) (value & 0xffff_ffff_ffff_fffful);
            l1 = (ulong) (value >> 64);

            if (negative)
            {
                l0 = ~l0;
                l1 = ~l1;
                l0++;
            }
        }
    }
}