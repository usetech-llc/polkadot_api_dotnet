using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Contracts
{
    public partial class InstantiateCall : IExtrinsicCall
    {
        // Rust type Compact<BalanceOf<T>>
        [Serialize(0)]
        [CompactBigIntegerConverter]
        public BigInteger Endowment { get; set; }


        // Rust type Compact<Gas>
        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger GasLimit { get; set; }


        // Rust type CodeHash<T>
        [Serialize(2)]
        public Hash CodeHash { get; set; }


        // Rust type Vec<u8>
        [Serialize(3)]
        [PrefixedArrayConverter]
        public byte[] Data { get; set; }


        // Rust type Vec<u8>
        [Serialize(4)]
        [PrefixedArrayConverter]
        public byte[] Salt { get; set; }



        public InstantiateCall() { }
        public InstantiateCall(BigInteger @endowment, BigInteger @gasLimit, Hash @codeHash, byte[] @data, byte[] @salt)
        {
            this.Endowment = @endowment;
            this.GasLimit = @gasLimit;
            this.CodeHash = @codeHash;
            this.Data = @data;
            this.Salt = @salt;
        }

    }
}