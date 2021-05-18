using OneOf;
using Polkadot.Api.Client.Model.DigestItemValues;
using Polkadot.Api.Client.Serialization;
using Polkadot.BinaryContracts;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model
{
    [BinaryJsonConverter]
    public class DigestItem<THash>
    {
        /// <summary>
        /// Other = 0,
        /// ChangesTrieRoot = 2,
        /// Consensus = 4,
        /// Seal = 5,
        /// PreRuntime = 6,
        /// ChangesTrieSignal = 7,
        /// </summary>
        [Serialize(0)]
        [OneOfConverter]
        public OneOf<Other, Empty, ChangesTrieRoot<THash>, Empty, Consensus, Seal, PreRuntime, ChangesTrieSignal> Value { get; set; }
    }
}