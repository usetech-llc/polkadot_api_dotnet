using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.Balances
{
    /// <summary>
    /// Some balance was reserved (moved from free to reserved).
    /// </summary>
    public class Reserved : IEvent
    {
        [Serialize(0)]
        public PublicKey Account;

        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger Amount;

        public Reserved()
        {
        }

        public Reserved(PublicKey account, BigInteger amount)
        {
            Account = account;
            Amount = amount;
        }
    }
}