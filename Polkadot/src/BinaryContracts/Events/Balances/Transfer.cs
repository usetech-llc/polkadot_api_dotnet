using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Balances
{
    /// <summary>
    /// Transfer succeeded (from, to, value).
    /// </summary>
    public class Transfer : IEvent
    {
        [Serialize(0)]
        public PublicKey From;

        [Serialize(1)]
        public PublicKey To;

        [Serialize(2)]
        [CompactBigIntegerConverter]
        public BigInteger Value;

        public Transfer()
        {
        }

        public Transfer(PublicKey @from, PublicKey to, BigInteger value)
        {
            From = @from;
            To = to;
            Value = value;
        }
    }
}