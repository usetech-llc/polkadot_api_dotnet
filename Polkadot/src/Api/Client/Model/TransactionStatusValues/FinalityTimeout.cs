using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client.Model.TransactionStatusValues
{
    /// Maximum number of finality watchers has been reached,
    /// old watchers are being removed.
    public struct FinalityTimeout<TBlockHash>
    {
        [Serialize(0)]
        public TBlockHash BlockHash { get; set; }
    }
}