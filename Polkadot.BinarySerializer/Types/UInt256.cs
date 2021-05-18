using System.Numerics;

namespace Polkadot.BinarySerializer.Types
{
    /// <summary>
    /// Warning: Not really an Uint256 in terms of arithmetics. Just a substitute for BinarySerializer to serialize/deserialize. 
    /// </summary>
    public struct UInt256
    {
        public BigInteger Value;

        public UInt256(ulong l0, ulong l1, ulong l2, ulong l3)
        {
            var val = new BigInteger(l3);
            val <<= 64;
            val |= l2;
            val <<= 64;
            val |= l1;
            val <<= 64;
            val |= l0;
            Value = val;
        }
        
        public static implicit operator BigInteger(UInt256 v)
        {
            return v.Value;
        }

        public static implicit operator UInt256(BigInteger v)
        {
            return new()
            {
                Value = v
            };
        }

        public void Deconstruct(out ulong l0, out ulong l1, out ulong l2, out ulong l3)
        {
            l0 = (ulong) (Value & 0xffff_ffff_ffff_fffful);
            l1 = (ulong) ((Value >> (64*1)) & 0xffff_ffff_ffff_fffful);
            l2 = (ulong) ((Value >> (64*2)) & 0xffff_ffff_ffff_fffful);
            l3 = (ulong) ((Value >> (64*3)) & 0xffff_ffff_ffff_fffful);
        }
    }
}