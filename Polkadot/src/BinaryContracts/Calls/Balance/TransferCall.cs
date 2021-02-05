using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.DataStructs;

namespace Polkadot.BinaryContracts.Calls.Balance
{
    public class TransferCall: IExtrinsicCall
    {
        [Serialize(0)]
        public PublicKey Destination { get; set; }
        
        [Serialize(1)]
        [CompactBigIntegerConverter]
        public BigInteger Amount { get; set; }

        public TransferCall()
        {
        }

        public TransferCall(PublicKey destination, BigInteger amount)
        {
            Destination = destination;
            Amount = amount;
        }
    }
}