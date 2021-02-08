using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Vesting
{
    public class VestOtherCall : IExtrinsicCall
    {
        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(0)]
        public PublicKey Target { get; set; }



        public VestOtherCall() { }
        public VestOtherCall(PublicKey @target)
        {
            this.Target = @target;
        }

    }
}