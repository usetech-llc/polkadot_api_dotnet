using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;
using Polkadot.Api.Client.Model;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public partial class CloseTipCall : IExtrinsicCall
    {
        // Rust type T::Hash
        [Serialize(0)]
        public Hash256 Hash { get; set; }



        public CloseTipCall() { }
        public CloseTipCall(Hash256 @hash)
        {
            this.Hash = @hash;
        }

    }
}