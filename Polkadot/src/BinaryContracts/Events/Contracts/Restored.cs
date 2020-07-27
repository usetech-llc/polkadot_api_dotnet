using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Contracts
{
    /// <summary>
    /// Restoration for a contract has been initiated.
    /// </summary>
    public class Restored : IEvent
    {
        /// <summary>
        /// `donor`: `AccountId`: Account ID of the restoring contract
        /// </summary>
        [Serialize(0)]
        public PublicKey Donor;

        /// <summary>
        /// `dest`: `AccountId`: Account ID of the restored contract
        /// </summary>
        [Serialize(1)]
        public PublicKey Dest;

        /// <summary>
        /// `code_hash`: `Hash`: Code hash of the restored contract
        /// </summary>
        [Serialize(2)]
        [PrefixedArrayConverter]
        public byte[] CodeHash;

        /// <summary>
        /// `rent_allowance: `Balance`: Rent allowance of the restored contract
        /// </summary>
        [Serialize(3)]
        [CompactBigIntegerConverter]
        public BigInteger RentAllowance;

        /// <summary>
        /// `success`: `bool`: True if the restoration was successful
        /// </summary>
        [Serialize(4)]
        public bool Success;

        public Restored()
        {
        }

        public Restored(PublicKey donor, PublicKey dest, byte[] codeHash, BigInteger rentAllowance, bool success)
        {
            Donor = donor;
            Dest = dest;
            CodeHash = codeHash;
            RentAllowance = rentAllowance;
            Success = success;
        }
    }
}