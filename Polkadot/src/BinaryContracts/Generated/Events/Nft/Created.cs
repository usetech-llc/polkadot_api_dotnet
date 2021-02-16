using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Nft
{
    public class Created : IEvent
    {
        // Rust type CollectionId
        [Serialize(0)]
        public uint EventArgument0 { get; set; }


        // Rust type u8
        [Serialize(1)]
        public byte EventArgument1 { get; set; }


        // Rust type AccountId
        [Serialize(2)]
        public PublicKey EventArgument2 { get; set; }



        public Created() { }
        public Created(uint @eventArgument0, byte @eventArgument1, PublicKey @eventArgument2)
        {
            this.EventArgument0 = @eventArgument0;
            this.EventArgument1 = @eventArgument1;
            this.EventArgument2 = @eventArgument2;
        }

    }
}