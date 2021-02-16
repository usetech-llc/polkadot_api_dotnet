using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common.CollectionModes
{
    public class Fungible
    {
        // Rust type DecimalPoints
        [Serialize(0)]
        public DecimalPoints Value { get; set; }



        public Fungible() { }
        public Fungible(DecimalPoints @value)
        {
            this.Value = @value;
        }

    }
}