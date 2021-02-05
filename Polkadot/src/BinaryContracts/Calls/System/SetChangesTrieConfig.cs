using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Types;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class SetChangesTrieConfig
    {
        [Serialize(0)]
        public Option<ChangesTrieConfiguration> ChangesTrieConfig { get; set; }
    }
}