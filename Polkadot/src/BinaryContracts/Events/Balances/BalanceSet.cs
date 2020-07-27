using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Balances
{
    /// <summary>
    /// A balance was set by root (who, free, reserved).
    /// </summary>
    public class BalanceSet : IEvent
    {
        [Serialize(0)]
        public PublicKey Who;

        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger Free;

        [Serialize(2)]
        [CompactBigIntegerConverter]
        public BigInteger Reserved;

        public BalanceSet()
        {
        }

        public BalanceSet(PublicKey who, BigInteger free, BigInteger reserved)
        {
            Who = who;
            Free = free;
            Reserved = reserved;
        }
    }
}