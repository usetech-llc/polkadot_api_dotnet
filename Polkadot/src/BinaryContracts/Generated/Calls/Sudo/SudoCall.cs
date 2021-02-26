using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Sudo
{
    public partial class SudoCall : IExtrinsicCall
    {
        // Rust type Box<<T as Trait>::Call>
        [Serialize(0)]
        public InheritanceCall<IExtrinsicCall> Call { get; set; }



        public SudoCall() { }
        public SudoCall(InheritanceCall<IExtrinsicCall> @call)
        {
            this.Call = @call;
        }

    }
}