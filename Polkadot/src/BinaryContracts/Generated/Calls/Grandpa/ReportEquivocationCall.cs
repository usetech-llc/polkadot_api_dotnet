using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Grandpa
{
    public class ReportEquivocationCall : IExtrinsicCall
    {
        // Rust type EquivocationProof<T::Hash, T::BlockNumber>
        [Serialize(0)]
        public EquivocationProof<Hash, BlockNumber> EquivocationProof { get; set; }


        // Rust type T::KeyOwnerProof
        [Serialize(1)]
        public KeyOwnerProof KeyOwnerProof { get; set; }



        public ReportEquivocationCall() { }
        public ReportEquivocationCall(EquivocationProof<Hash, BlockNumber> @equivocationProof, KeyOwnerProof @keyOwnerProof)
        {
            this.EquivocationProof = @equivocationProof;
            this.KeyOwnerProof = @keyOwnerProof;
        }

    }
}