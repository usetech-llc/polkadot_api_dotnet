using OneOf;
using Polkadot.Api.Client.Serialization;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinarySerializer.Types;

namespace Polkadot.Api.Client.Model
{
    [BinaryJsonConverter]
    // it's untagged
    public class NumberOrHex
    {
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<ulong, UInt256> Value { get; set; }

        public NumberOrHex()
        {
        }
        
        public NumberOrHex(UInt256 value)
        {
            Value = value;
        }

        public NumberOrHex(ulong value)
        {
            Value = value;
        }

        public static implicit operator NumberOrHex(ulong value)
        {
            return new()
            {
                Value = value
            };
        }
        
        public static implicit operator NumberOrHex(UInt256 value)
        {
            return new()
            {
                Value = value
            };
        }
    }
}