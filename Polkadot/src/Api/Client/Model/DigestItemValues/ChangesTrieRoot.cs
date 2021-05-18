using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client.Model.DigestItemValues
{
    /// System digest item that contains the root of changes trie at given
    /// block. It is created for every block iff runtime supports changes
    /// trie creation.
    public class ChangesTrieRoot<THash>
    {
        [Serialize(0)]
        public THash Hash { get; set; }
    }
}