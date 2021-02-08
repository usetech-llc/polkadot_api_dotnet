using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class SetHeapPagesCall : IExtrinsicCall
    {
        // Rust type u64
        [Serialize(0)]
        public ulong Pages { get; set; }



        public SetHeapPagesCall() { }
        public SetHeapPagesCall(ulong @pages)
        {
            this.Pages = @pages;
        }

    }
}