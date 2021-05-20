using OneOf;
using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client.Model.DigestItemValues
{
    /// Digest item that contains signal from changes tries manager to the
    /// native code.
    public class ChangesTrieSignal
    {
        [Serialize(0)]
        public OneOf<NewConfiguration> Value { get; set; }
    }
}