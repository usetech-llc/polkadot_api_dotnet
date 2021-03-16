using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class Key
    {
        [Serialize(0)]
        public byte[] Value { get; set; }
        
        public static implicit operator byte[](Key k)
        {
            return k.Value;
        }

        public static implicit operator Key(byte[] b)
        {
            return new Key()
            {
                Value = b
            };
        }
    }
}