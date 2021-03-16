using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Nft
{
    public partial class Transfer : IEvent
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint EventArgument0 { get; set; }


        // Rust type TokenId
        [Serialize(1)]
        public uint EventArgument1 { get; set; }


        // Rust type AccountId
        [Serialize(2)]
        public PublicKey EventArgument2 { get; set; }


        // Rust type AccountId
        [Serialize(3)]
        public PublicKey EventArgument3 { get; set; }


        // Rust type u128
        [Serialize(4)]
        [U128Converter]
        public BigInteger EventArgument4 { get; set; }



        public Transfer() { }
        public Transfer(uint @eventArgument0, uint @eventArgument1, PublicKey @eventArgument2, PublicKey @eventArgument3, BigInteger @eventArgument4)
        {
            this.EventArgument0 = @eventArgument0;
            this.EventArgument1 = @eventArgument1;
            this.EventArgument2 = @eventArgument2;
            this.EventArgument3 = @eventArgument3;
            this.EventArgument4 = @eventArgument4;
        }

    }
}