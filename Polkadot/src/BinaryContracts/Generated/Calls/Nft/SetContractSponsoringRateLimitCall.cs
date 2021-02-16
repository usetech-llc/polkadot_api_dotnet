using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Nft
{
    public class SetContractSponsoringRateLimitCall : IExtrinsicCall
    {
        // Rust type T::AccountId
        [Serialize(0)]
        public PublicKey ContractAddress { get; set; }


        // Rust type T::BlockNumber
        [Serialize(1)]
        public BlockNumber RateLimit { get; set; }



        public SetContractSponsoringRateLimitCall() { }
        public SetContractSponsoringRateLimitCall(PublicKey @contractAddress, BlockNumber @rateLimit)
        {
            this.ContractAddress = @contractAddress;
            this.RateLimit = @rateLimit;
        }

    }
}