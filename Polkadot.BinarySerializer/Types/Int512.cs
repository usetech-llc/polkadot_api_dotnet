using System.Numerics;

namespace Polkadot.BinarySerializer.Types
{
    /// <summary>
    /// Warning: Not really an Int512 in terms of arithmetics. Just a substitute for BinarySerializer to serialize/deserialize. 
    /// </summary>
    public struct Int512
    {
        public BigInteger Value;

        public Int512(ulong l0, ulong l1, ulong l2, ulong l3, ulong l4, ulong l5, ulong l6, ulong l7)
        {
            var negative = (l7 & 0x8000_0000_0000_0000ul) == 0x8000_0000_0000_0000ul;
            if (negative)
            {
                l0--;
                l0 = ~l0;
                l1 = ~l1;
                l2 = ~l2;
                l3 = ~l3;
                l4 = ~l4;
                l5 = ~l5;
                l6 = ~l6;
                l7 = ~l7;
            }

            var val = new BigInteger(l7);
            val <<= 64;
            val |= l6;
            val <<= 64;
            val |= l5;
            val <<= 64;
            val |= l4;
            val <<= 64;
            val |= l3;
            val <<= 64;
            val |= l2;
            val <<= 64;
            val |= l1;
            val <<= 64;
            val |= l0;
            Value = negative ? -val : val;
        }
        
        public static implicit operator BigInteger(Int512 v)
        {
            return v.Value;
        }

        public static implicit operator Int512(BigInteger v)
        {
            return new()
            {
                Value = v
            };
        }

        public void Deconstruct(out ulong l0, out ulong l1, out ulong l2, out ulong l3, out ulong l4, out ulong l5, out ulong l6, out ulong l7)
        {
            var negative = Value < 0;
            var value = negative ? -Value : Value;
            
            
            l0 = (ulong) (value & 0xffff_ffff_ffff_fffful);
            l1 = (ulong) ((value >> (64*1)) & 0xffff_ffff_ffff_fffful);
            l2 = (ulong) ((value >> (64*2)) & 0xffff_ffff_ffff_fffful);
            l3 = (ulong) ((value >> (64*3)) & 0xffff_ffff_ffff_fffful);
            l4 = (ulong) ((value >> (64*4)) & 0xffff_ffff_ffff_fffful);
            l5 = (ulong) ((value >> (64*5)) & 0xffff_ffff_ffff_fffful);
            l6 = (ulong) ((value >> (64*6)) & 0xffff_ffff_ffff_fffful);
            l7 = (ulong) ((value >> (64*7)) & 0xffff_ffff_ffff_fffful);

            if (negative)
            {
                l0 = ~l0;
                l1 = ~l1;
                l2 = ~l2;
                l3 = ~l3;
                l4 = ~l4;
                l5 = ~l5;
                l6 = ~l6;
                l7 = ~l7;
                l0++;
            }
        }
    }
}