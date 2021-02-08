using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using System.Numerics;

namespace Polkadot.BinaryContracts.Calls.Grandpa
{
    public class NoteStalledCall : IExtrinsicCall
    {
        // Rust type T::BlockNumber
        [Serialize(0)]
        public BlockNumber Delay { get; set; }


        // Rust type T::BlockNumber
        [Serialize(1)]
        public BlockNumber BestFinalizedBlockNumber { get; set; }



        public NoteStalledCall() { }
        public NoteStalledCall(BlockNumber @delay, BlockNumber @bestFinalizedBlockNumber)
        {
            this.Delay = @delay;
            this.BestFinalizedBlockNumber = @bestFinalizedBlockNumber;
        }

    }
}