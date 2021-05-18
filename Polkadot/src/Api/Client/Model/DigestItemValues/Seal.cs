using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model.DigestItemValues
{
    /// Put a Seal on it. This is only used by native code, and is never seen
    /// by runtimes.
    public class Seal
    {
        [Serialize(0)]
        public ConsensusEngineId Id { get; set; }
        [Serialize(1)]
        [PrefixedArrayConverter]
        public byte[] Something { get; set; }
    }
}