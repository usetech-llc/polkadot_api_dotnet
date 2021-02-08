using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Contracts
{
    public class ClaimSurchargeCall : IExtrinsicCall
    {
        // Rust type T::AccountId
        [Serialize(0)]
        public PublicKey Dest { get; set; }


        // Rust type Option<T::AccountId>
        [Serialize(1)]
        [OneOfConverter]
        public OneOf.OneOf<OneOf.Types.None, PublicKey> AuxSender { get; set; }



        public ClaimSurchargeCall() { }
        public ClaimSurchargeCall(PublicKey @dest, OneOf.OneOf<OneOf.Types.None, PublicKey> @auxSender)
        {
            this.Dest = @dest;
            this.AuxSender = @auxSender;
        }

    }
}