using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public class ReFungibleItemType
    {
        // Rust type "Vec<Ownership<AccountId>>"
        [Serialize(0)]
        [PrefixedArrayConverter]
        public Ownership[] Owner { get; set; }


        // Rust type "Vec<u8>"
        [Serialize(1)]
        [PrefixedArrayConverter]
        public byte[] ConstData { get; set; }


        // Rust type "Vec<u8>"
        [Serialize(2)]
        [PrefixedArrayConverter]
        public byte[] VariableData { get; set; }



        public ReFungibleItemType() { }
        public ReFungibleItemType(Ownership[] @owner, byte[] @constData, byte[] @variableData)
        {
            this.Owner = @owner;
            this.ConstData = @constData;
            this.VariableData = @variableData;
        }

    }
}