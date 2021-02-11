using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class RemoveFromContractWhiteListCall : IExtrinsicCall
    {
        // Rust type T::AccountId
        [Serialize(0)]
        public PublicKey ContractAddress { get; set; }


        // Rust type T::AccountId
        [Serialize(1)]
        public PublicKey AccountAddress { get; set; }



        public RemoveFromContractWhiteListCall() { }
        public RemoveFromContractWhiteListCall(PublicKey @contractAddress, PublicKey @accountAddress)
        {
            this.ContractAddress = @contractAddress;
            this.AccountAddress = @accountAddress;
        }

    }
}