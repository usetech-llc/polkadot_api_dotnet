using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class CreateNftData
    {
        // Rust type "Vec<u8>"
        [Serialize(0)]
        [PrefixedArrayConverter]
        public byte[] ConstData { get; set; }


        // Rust type "Vec<u8>"
        [Serialize(1)]
        [PrefixedArrayConverter]
        public byte[] VariableData { get; set; }



        public CreateNftData() { }
        public CreateNftData(byte[] @constData, byte[] @variableData)
        {
            this.ConstData = @constData;
            this.VariableData = @variableData;
        }

    }
}