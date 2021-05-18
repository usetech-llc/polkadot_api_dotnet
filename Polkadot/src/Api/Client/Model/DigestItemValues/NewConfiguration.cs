using Polkadot.BinaryContracts;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Types;

namespace Polkadot.Api.Client.Model.DigestItemValues
{
    /// New changes trie configuration is enacted, starting from **next block**.
    ///
    /// The block that emits this signal will contain changes trie (CT) that covers
    /// blocks range [BEGIN; current block], where BEGIN is (order matters):
    /// - LAST_TOP_LEVEL_DIGEST_BLOCK+1 if top level digest CT has ever been created
    ///   using current configuration AND the last top level digest CT has been created
    ///   at block LAST_TOP_LEVEL_DIGEST_BLOCK;
    /// - LAST_CONFIGURATION_CHANGE_BLOCK+1 if there has been CT configuration change
    ///   before and the last configuration change happened at block
    ///   LAST_CONFIGURATION_CHANGE_BLOCK;
    /// - 1 otherwise.
    public class NewConfiguration
    {
        [Serialize(0)]
        public Option<ChangesTrieConfiguration> Value { get; set; }
    }
}