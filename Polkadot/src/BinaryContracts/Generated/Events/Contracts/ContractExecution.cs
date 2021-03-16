using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Events.Contracts
{
    public partial class ContractExecution : IEvent
    {
        // Rust type AccountId
        [Serialize(0)]
        public PublicKey EventArgument0 { get; set; }


        // Rust type Vec<u8>
        [Serialize(1)]
        [PrefixedArrayConverter]
        public byte[] EventArgument1 { get; set; }



        public ContractExecution() { }
        public ContractExecution(PublicKey @eventArgument0, byte[] @eventArgument1)
        {
            this.EventArgument0 = @eventArgument0;
            this.EventArgument1 = @eventArgument1;
        }

    }
}