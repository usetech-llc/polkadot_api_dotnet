using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model
{
    public class Justification
    {
        [PrefixedArrayConverter]
        public byte[] Value { get; set; }
    }
}