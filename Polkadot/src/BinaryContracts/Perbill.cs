using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public struct Perbill
    {
        [Serialize(0)]
        public uint Value { get; set; }

        public Perbill(uint value)
        {
            Value = value;
        }

        public static implicit operator uint(Perbill p)
        {
            return p.Value;
        }

        public static implicit operator Perbill(uint v)
        {
            return new Perbill(v);
        }
    }
}