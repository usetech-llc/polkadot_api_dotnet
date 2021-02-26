using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public partial class ToggleContractWhiteListCall : IExtrinsicCall
    {
        // Rust type T::AccountId
        [Serialize(0)]
        public PublicKey ContractAddress { get; set; }


        // Rust type bool
        [Serialize(1)]
        public bool Enable { get; set; }



        public ToggleContractWhiteListCall() { }
        public ToggleContractWhiteListCall(PublicKey @contractAddress, bool @enable)
        {
            this.ContractAddress = @contractAddress;
            this.Enable = @enable;
        }

    }
}