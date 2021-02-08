using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Treasury
{
    public class ProposeSpendCall : IExtrinsicCall
    {
        // Rust type Compact<BalanceOf<T, I>>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger Value { get; set; }


        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(1)]
        public PublicKey Beneficiary { get; set; }



        public ProposeSpendCall() { }
        public ProposeSpendCall(BigInteger @value, PublicKey @beneficiary)
        {
            this.Value = @value;
            this.Beneficiary = @beneficiary;
        }

    }
}