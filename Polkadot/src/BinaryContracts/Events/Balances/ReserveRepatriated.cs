using System.Numerics;
using Polkadot.BinaryContracts.Events.BalanceStatusEnum;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Balances
{
    /// <summary>
    /// Some balance was moved from the reserve of the first account to the second account.
    /// Final argument indicates the destination balance type.
    /// </summary>
    public class ReserveRepatriated : IEvent
    {
        [Serialize(0)]
        public PublicKey FirstAccount;

        [Serialize(1)]
        public PublicKey SecondAccount;

        [Serialize(2)]
        [CompactBigIntegerConverter]
        public BigInteger Amount;

        [Serialize(3)]
        public BalanceStatus Status;

        public ReserveRepatriated()
        {
        }

        public ReserveRepatriated(PublicKey firstAccount, PublicKey secondAccount, BigInteger amount, BalanceStatus status)
        {
            FirstAccount = firstAccount;
            SecondAccount = secondAccount;
            Amount = amount;
            Status = status;
        }
    }
}