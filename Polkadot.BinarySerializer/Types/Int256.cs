using System.Numerics;

namespace Polkadot.BinarySerializer.Types
{
    /// <summary>
    /// Warning: Not really an Int256 in terms of arithmetics. Just a substitute for BinarySerializer to serialize/deserialize. 
    /// </summary>
    public struct Int256
    {
        public BigInteger Value;

        public Int256(ulong l0, ulong l1, ulong l2, ulong l3)
        {
            var negative = (l3 & 0x8000_0000_0000_0000ul) == 0x8000_0000_0000_0000ul;
            if (negative)
            {
                l0--;
                l0 = ~l0;
                l1 = ~l1;
                l2 = ~l2;
                l3 = ~l3;
            }

            var val = new BigInteger(l3);
            val <<= 64;
            val |= l2;
            val <<= 64;
            val |= l1;
            val <<= 64;
            val |= l0;
            Value = negative ? -val : val;
        }
        
        public static implicit operator BigInteger(Int256 v)
        {
            return v.Value;
        }

        public static implicit operator Int256(BigInteger v)
        {
            return new()
            {
                Value = v
            };
        }

        public void Deconstruct(out ulong l0, out ulong l1, out ulong l2, out ulong l3)
        {
            var negative = Value < 0;
            var value = negative ? -Value : Value;
            
            
            l0 = (ulong) (value & 0xffff_ffff_ffff_fffful);
            l1 = (ulong) ((value >> (64*1)) & 0xffff_ffff_ffff_fffful);
            l2 = (ulong) ((value >> (64*2)) & 0xffff_ffff_ffff_fffful);
            l3 = (ulong) ((value >> (64*3)) & 0xffff_ffff_ffff_fffful);

            if (negative)
            {
                l0 = ~l0;
                l1 = ~l1;
                l2 = ~l2;
                l3 = ~l3;
                l0++;
            }
        }
    }
}