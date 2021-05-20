using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client.Model.TransactionStatusValues
{
    /// Transaction has been finalized by a finality-gadget, e.g GRANDPA
    public struct Finalized<TBlockHash>
    {
        [Serialize(0)]
        public TBlockHash BlockHash { get; set; }
    }
}