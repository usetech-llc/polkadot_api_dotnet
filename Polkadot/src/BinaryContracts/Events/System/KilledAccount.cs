using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events
{
    public class KilledAccount
    {
        [Serialize(0)]
        public PublicKey Account;

        public KilledAccount()
        {
        }

        public KilledAccount(PublicKey account)
        {
            Account = account;
        }
    }
}