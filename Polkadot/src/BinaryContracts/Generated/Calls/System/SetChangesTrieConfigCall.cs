using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class SetChangesTrieConfigCall : IExtrinsicCall
    {
        // Rust type Option<ChangesTrieConfiguration>
        [Serialize(0)]
        [OneOfConverter]
        public OneOf.OneOf<OneOf.Types.None, ChangesTrieConfiguration> ChangesTrieConfig { get; set; }



        public SetChangesTrieConfigCall() { }
        public SetChangesTrieConfigCall(OneOf.OneOf<OneOf.Types.None, ChangesTrieConfiguration> @changesTrieConfig)
        {
            this.ChangesTrieConfig = @changesTrieConfig;
        }

    }
}