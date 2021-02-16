using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Sudo
{
    public class SudoAsCall : IExtrinsicCall
    {
        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(0)]
        public PublicKey Who { get; set; }


        // Rust type Box<<T as Trait>::Call>
        [Serialize(1)]
        public InheritanceCall<IExtrinsicCall> Call { get; set; }



        public SudoAsCall() { }
        public SudoAsCall(PublicKey @who, InheritanceCall<IExtrinsicCall> @call)
        {
            this.Who = @who;
            this.Call = @call;
        }

    }
}