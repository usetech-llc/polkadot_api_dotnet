using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;
using Polkadot.Api.Client.Model;

namespace Polkadot.BinaryContracts.Calls.Grandpa
{
    public partial class ReportEquivocationCall : IExtrinsicCall
    {
        // Rust type EquivocationProof<T::Hash, T::BlockNumber>
        [Serialize(0)]
        public EquivocationProof<Hash256, BlockNumber> EquivocationProof { get; set; }


        // Rust type T::KeyOwnerProof
        [Serialize(1)]
        public KeyOwnerProof KeyOwnerProof { get; set; }



        public ReportEquivocationCall() { }
        public ReportEquivocationCall(EquivocationProof<Hash256, BlockNumber> @equivocationProof, KeyOwnerProof @keyOwnerProof)
        {
            this.EquivocationProof = @equivocationProof;
            this.KeyOwnerProof = @keyOwnerProof;
        }

    }
}