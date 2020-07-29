using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Balances
{
    /// <summary>
    /// Some balance was unreserved (moved from reserved to free).
    /// </summary>
    public class Unreserved : IEvent
    {
        [Serialize(0)]
        public PublicKey Account;

        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger Amount;

        public Unreserved()
        {
        }

        public Unreserved(PublicKey account, BigInteger amount)
        {
            Account = account;
            Amount = amount;
        }
    }
}