using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public class CollectionId
    {
        // Rust type u32
        [Serialize(0)]
        public uint Value { get; set; }



        public CollectionId() { }
        public CollectionId(uint @value)
        {
            this.Value = @value;
        }

    }
}