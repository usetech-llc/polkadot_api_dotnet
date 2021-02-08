using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class SetCodeWithoutChecksCall : IExtrinsicCall
    {
        // Rust type Vec<u8>
        [Serialize(0)]
        [PrefixedArrayConverter]
        public byte[] Code { get; set; }



        public SetCodeWithoutChecksCall() { }
        public SetCodeWithoutChecksCall(byte[] @code)
        {
            this.Code = @code;
        }

    }
}