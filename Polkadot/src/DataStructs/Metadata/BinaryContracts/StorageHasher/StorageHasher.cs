using OneOf;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.DataStructs.Metadata.BinaryContracts.StorageHasher
{
    public struct StorageHasher
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<
            Blake2_128,
            Blake2_256,
            Blake2_128Concat,
            Twox128,
            Twox256,
            Twox64Concat,
            Identity> 
            Value { get; set; }
    }
}