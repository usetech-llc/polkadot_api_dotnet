using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Balances
{
    /// <summary>
    /// An account was removed whose balance was non-zero but below ExistentialDeposit,
    /// resulting in an outright loss.
    /// </summary>
    public class DustLost : IEvent
    {
        [Serialize(0)]
        public PublicKey Account;

        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger Balance;

        public DustLost()
        {
        }

        public DustLost(PublicKey account, BigInteger balance)
        {
            Account = account;
            Balance = balance;
        }
    }
}