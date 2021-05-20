using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client.Model.TransactionStatusValues
{
    /// The block this transaction was included in has been retracted.
    public struct Retracted<TBlockHash>
    {
        [Serialize(0)]
        public TBlockHash BlockHash { get; set; }
    }
}