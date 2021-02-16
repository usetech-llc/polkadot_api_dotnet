using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.System
{
    public class RemarkCall : IExtrinsicCall
    {
        // Rust type Vec<u8>
        [Serialize(0)]
        [PrefixedArrayConverter]
        public byte[] _remark { get; set; }



        public RemarkCall() { }
        public RemarkCall(byte[] @_remark)
        {
            this._remark = @_remark;
        }

    }
}