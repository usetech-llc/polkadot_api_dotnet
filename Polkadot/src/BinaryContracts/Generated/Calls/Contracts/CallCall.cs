using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Contracts
{
    public class CallCall : IExtrinsicCall
    {
        // Rust type <T::Lookup as StaticLookup>::Source
        [Serialize(0)]
        public PublicKey Dest { get; set; }


        // Rust type Compact<BalanceOf<T>>
        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger Value { get; set; }


        // Rust type Compact<Gas>
        [Serialize(2)]
        [CompactBigIntegerConverter]
        public BigInteger GasLimit { get; set; }


        // Rust type Vec<u8>
        [Serialize(3)]
        [PrefixedArrayConverter]
        public byte[] Data { get; set; }



        public CallCall() { }
        public CallCall(PublicKey @dest, BigInteger @value, BigInteger @gasLimit, byte[] @data)
        {
            this.Dest = @dest;
            this.Value = @value;
            this.GasLimit = @gasLimit;
            this.Data = @data;
        }

    }
}