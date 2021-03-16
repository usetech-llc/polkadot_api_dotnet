using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.System
{
    public partial class SetStorageCall : IExtrinsicCall
    {
        // Rust type Vec<KeyValue>
        [Serialize(0)]
        [PrefixedArrayConverter]
        public KeyValue[] Items { get; set; }



        public SetStorageCall() { }
        public SetStorageCall(KeyValue[] @items)
        {
            this.Items = @items;
        }

    }
}