using Polkadot.BinarySerializer;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Events.System
{
    public class KilledAccount : IEvent
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