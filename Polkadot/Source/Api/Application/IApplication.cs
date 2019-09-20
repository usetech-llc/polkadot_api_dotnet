namespace Polkadot.Api
{
    using System;
    using System.Threading.Tasks;
    using Polkadot.Data;

    public interface IApplication : IDisposable
    {
        int Connect(string node_url = "");

        void Disconnect();

        /// <summary>
        /// Call 4 methods and put them together in a single object
        /// system_chain
        /// system_name
        /// system_version
        /// system_properties
        /// </summary>
        SystemInfo GetSystemInfo();

        /// <summary>
        ///  Retreives the block hash for specific block
        /// </summary>
        /// <param name=""> struct with blockNumber block number </param>
        /// <returns> BlockHash struct with result </returns>
        BlockHash GetBlockHash(GetBlockHashParams param);

        /// <summary>
        ///  Retreives the runtime version information for specific block
        /// </summary>
        /// <param name=""> struct with blockHash 64 diget number in hex format </param>
        /// <returns> RuntimeVersion struct with result </returns>            
        RuntimeVersion GetRuntimeVersion(GetRuntimeVersionParams param);

        /// <summary>
        ///  Get header and body of a relay chain block
        /// </summary>
        /// <param name=""> struct with blockHash 64 diget number in hex format </param>
        /// <returns> SignedBlock struct with result </returns>     
        SignedBlock GetBlock(GetBlockParams param);


        /// <summary>
        /// Retrieves the header for a specific block
        /// </summary>
        /// <param name="param"> struct with blockHash 64 diget number in hex format </param>
        /// <returns> BlockHeader struct with result </returns>
        BlockHeader GetBlockHeader(GetBlockParams param);

        /// <summary>
        /// Returns current state of the network
        /// </summary>
        /// <returns>
        /// NetworkState struct with result
        /// </returns>
        NetworkState GetNetworkState();

        /// <summary>
        /// Get hash of the last finalized block in the chain
        /// </summary>
        /// <returns> FinalHead struct with result </returns>
        FinalHead GetFinalizedHead();


        /**
        *  Retreives the runtime metadata for specific block
        *
        *  @param struct with blockHash 64 diget number in hex format
        *  @return Metadata struct with result
        */

        //Metadata GetMetadata(GetMetadataParams param);
        string GetMetadata(GetMetadataParams param);


        /// <summary>
        /// Subscribe to most recent block number.Only one subscription at a time is allowed.If a subscription already 
        /// exists, old subscription will be discarded and replaced with the new one.Until unsubscribeBlockNumber method is
        /// called, the API will be receiving updates and forwarding them to subscribed object/function.Only
        /// unsubscribeBlockNumber will physically unsubscribe from WebSocket endpoint updates.
        /// </summary>
        /// <param name="callback"> callback - delegate that will receive updates</param>
        /// <returns> operation result </returns>
        int SubscribeBlockNumber(Action<long> callback);

        /// <summary>

        /// </summary>
        /// <param name="callback"> callback - delegate that will receive updates</param>
        /// <returns> operation result </returns>
        void UnsubscribeBlockNumber(int id);
    }
}
