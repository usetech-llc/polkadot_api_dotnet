using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Balances
{
    public class SetBalanceCall : IExtrinsicCall
    {
        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(0)]
        public PublicKey Who { get; set; }


        // Rust type Compact<T::Balance>
        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger NewFree { get; set; }


        // Rust type Compact<T::Balance>
        [Serialize(2)]
        [CompactBigIntegerConverter]
        public BigInteger NewReserved { get; set; }



        public SetBalanceCall() { }
        public SetBalanceCall(PublicKey @who, BigInteger @newFree, BigInteger @newReserved)
        {
            this.Who = @who;
            this.NewFree = @newFree;
            this.NewReserved = @newReserved;
        }

    }
}