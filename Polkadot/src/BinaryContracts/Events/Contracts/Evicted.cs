using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Contracts
{
    /// <summary>
    /// Contract has been evicted and is now in tombstone state.
    /// </summary>
    public class Evicted : IEvent
    {
        /// <summary>
        /// `contract`: `AccountId`: The account ID of the evicted contract.
        /// </summary>
        [Serialize(0)]
        public PublicKey Contract;

        /// <summary>
        /// `tombstone`: `bool`: True if the evicted contract left behind a tombstone.
        /// </summary>
        [Serialize(1)]
        public bool Tombstone;

        public Evicted()
        {
        }

        public Evicted(PublicKey contract, bool tombstone)
        {
            Contract = contract;
            Tombstone = tombstone;
        }
    }
}