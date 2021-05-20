using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model.TransactionStatusValues
{
    /// The transaction has been broadcast to the given peers.
    public struct Broadcast
    {
        [PrefixedArrayConverter(ItemConverter = typeof(Utf8StringConverter))]
        public string[] Peers { get; set; }
    }
}