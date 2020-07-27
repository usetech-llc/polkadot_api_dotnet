using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Contracts
{
    /// <summary>
    /// Transfer happened `from` to `to` with given `value` as part of a `call` or `instantiate`.
    /// </summary>
    public class Transfer : IEvent
    {
        [Serialize(0)]
        public PublicKey From;
        
        [Serialize(1)]
        public PublicKey To;

        [Serialize(2)]
        [CompactBigIntegerConverter]
        public BigInteger Amount;

        public Transfer()
        {
        }

        public Transfer(PublicKey @from, PublicKey to, BigInteger amount)
        {
            From = @from;
            To = to;
            Amount = amount;
        }
    }
}