using Polkadot.Api.Client.Serialization;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model
{
    [BinaryJsonConverter]
    public class AccountId32
    {
        [Serialize(0)]
        [FixedSizeArrayConverter(32)]
        public byte[] PublicKey { get; set; } 
    }
}