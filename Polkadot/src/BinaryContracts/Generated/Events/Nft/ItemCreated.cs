using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Nft
{
    public partial class ItemCreated : IEvent
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



        public ItemCreated() { }
        public ItemCreated(uint @eventArgument0, uint @eventArgument1, PublicKey @eventArgument2)
        {
            this.EventArgument0 = @eventArgument0;
            this.EventArgument1 = @eventArgument1;
            this.EventArgument2 = @eventArgument2;
        }

    }
}