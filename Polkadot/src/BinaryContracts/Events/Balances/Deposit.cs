using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Balances
{
    /// <summary>
    /// Some amount was deposited (e.g. for transaction fees).
    /// </summary>
    public class Deposit : IEvent
    {
        [Serialize(0)]
        public PublicKey Account;

        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger Balance;

        public Deposit()
        {
        }

        public Deposit(PublicKey account, BigInteger balance)
        {
            Account = account;
            Balance = balance;
        }
    }
}