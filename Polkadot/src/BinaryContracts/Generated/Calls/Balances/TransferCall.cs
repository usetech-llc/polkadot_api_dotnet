using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Balances
{
    public class TransferCall : IExtrinsicCall
    {
        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(0)]
        public PublicKey Dest { get; set; }


        // Rust type Compact<T::Balance>
        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger Value { get; set; }



        public TransferCall() { }
        public TransferCall(PublicKey @dest, BigInteger @value)
        {
            this.Dest = @dest;
            this.Value = @value;
        }

    }
}