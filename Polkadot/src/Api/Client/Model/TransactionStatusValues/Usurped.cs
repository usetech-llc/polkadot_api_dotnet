using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client.Model.TransactionStatusValues
{
    /// Transaction has been replaced in the pool, by another transaction
    /// that provides the same tags. (e.g. same (sender, nonce)).
    public struct Usurped<THash>
    {
        [Serialize(0)]
        public THash Hash { get; set; }
    }
}