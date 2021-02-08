using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class FillBlockCall : IExtrinsicCall
    {
        // Rust type Perbill
        [Serialize(0)]
        public Perbill Ratio { get; set; }



        public FillBlockCall() { }
        public FillBlockCall(Perbill @ratio)
        {
            this.Ratio = @ratio;
        }

    }
}