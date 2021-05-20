using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client.Model.TransactionStatusValues
{
    /// Transaction has been included in block with given hash.
    public struct InBlock<TBlockHash>
    {
        [Serialize(0)]
        public TBlockHash BlockHash { get; set; }
    }
}