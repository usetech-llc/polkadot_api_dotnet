using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Balances
{
    /// <summary>
    /// An account was created with some free balance.
    /// </summary>
    public class Endowed : IEvent
    {
        /// <summary>
        /// An account was created with some free balance.
        /// </summary>
        [Serialize(0)]
        public PublicKey Account;

        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger Balance;

        public Endowed()
        {
        }

        public Endowed(PublicKey account, BigInteger balance)
        {
            Account = account;
            Balance = balance;
        }
    }
}