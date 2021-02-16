using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Balances
{
    public class TransferKeepAliveCall : IExtrinsicCall
    {
        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(0)]
        public PublicKey Dest { get; set; }


        // Rust type Compact<T::Balance>
        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger Value { get; set; }



        public TransferKeepAliveCall() { }
        public TransferKeepAliveCall(PublicKey @dest, BigInteger @value)
        {
            this.Dest = @dest;
            this.Value = @value;
        }

    }
}