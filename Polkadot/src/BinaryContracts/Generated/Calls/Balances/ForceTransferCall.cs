using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Balances
{
    public class ForceTransferCall : IExtrinsicCall
    {
        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(0)]
        public PublicKey Source { get; set; }


        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(1)]
        public PublicKey Dest { get; set; }


        // Rust type Compact<T::Balance>
        [Serialize(2)]
        [CompactBigIntegerConverter]
        public BigInteger Value { get; set; }



        public ForceTransferCall() { }
        public ForceTransferCall(PublicKey @source, PublicKey @dest, BigInteger @value)
        {
            this.Source = @source;
            this.Dest = @dest;
            this.Value = @value;
        }

    }
}