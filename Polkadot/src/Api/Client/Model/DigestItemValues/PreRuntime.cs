using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model.DigestItemValues
{
    /// A pre-runtime digest.
    ///
    /// These are messages from the consensus engine to the runtime, although
    /// the consensus engine can (and should) read them itself to avoid
    /// code and state duplication. It is erroneous for a runtime to produce
    /// these, but this is not (yet) checked.
    ///
    /// NOTE: the runtime is not allowed to panic or fail in an `on_initialize`
    /// call if an expected `PreRuntime` digest is not present. It is the
    /// responsibility of a external block verifier to check this. Runtime API calls
    /// will initialize the block without pre-runtime digests, so initialization
    /// cannot fail when they are missing.
    public class PreRuntime
    {
        [Serialize(0)]
        public ConsensusEngineId Id { get; set; }
        [Serialize(1)]
        [PrefixedArrayConverter]
        public byte[] Something { get; set; }
    }
}