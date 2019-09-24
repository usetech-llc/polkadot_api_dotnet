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
        ///  Generates storage key for a certain Module and State variable defined by parameter and prefix. Parameter is a JSON
        ///   string representing a value of certain type, which has two fields: type and value.Type should be one of type
        ///   strings defined above.Value should correspond to the type.Example:
        ///
        ///      { "type" : "AccountId", "value" : "5ECcjykmdAQK71qHBCkEWpWkoMJY6NXvpdKy8UeMx16q5gFr"}
        ///
        ///   Information about Modules and State variables(with parameters and their types) is returned by getMetadata
        ///   method.
        /// </summary>
        /// <param name="jsonPrm"> JSON string that contains parameter and its type</param>
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="variable"> state variable (as in metadata for given module)</param>
        /// <returns> Storage key </returns>
        string GetKeys(string jsonPrm, string module, string variable);

        /// <summary>
        ///  Reads storage for a certain Module and State variable defined by parameter and prefix.Parameter is a JSON
        ///   string representing a value of certain type, which has two fields: type and value.Type should be one of type
        ///   strings defined above.Value should correspond to the type.Example:
        ///
        ///      { "type" : "AccountId", "value" : "5ECcjykmdAQK71qHBCkEWpWkoMJY6NXvpdKy8UeMx16q5gFr"}
        ///
        ///   Information about Modules and State variables(with parameters and their types) is returned by getMetadata
        ///   method.
        /// </summary>
        /// <param name="jsonPrm"> JSON string that contains parameter and its type</param>
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="variable"> state variable (as in metadata for given module)</param>
        /// <returns> Storage content </returns>
        string GetStorage(string jsonPrm, string module, string variable);

        /// <summary>
        /// Returns storage hash of given State Variable for a given Module defined by parameter.
        ///  Parameter is a JSON string representing a value of certain type, which has two fields: type and value.Type
        ///  should be one of type strings defined above.Value should correspond to the type. Example:
        ///
        ///      { "type" : "AccountId", "value" : "5ECcjykmdAQK71qHBCkEWpWkoMJY6NXvpdKy8UeMx16q5gFr"}
        ///
        ///   Information about Modules and State variables(with parameters and their types) is returned by getMetadata
        ///   method.
        /// </summary>
        /// <param name="jsonPrm"> JSON string that contains parameter and its type</param>
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="variable"> state variable (as in metadata for given module)</param>
        /// <returns> Storage hash </returns>
        string GetStorageHash(string jsonPrm, string module, string variable);

        /// <summary>
        /// Returns storage size for a given State Variable for a given Module defined by parameter.
        ///  Parameter is a JSON string representing a value of certain type, which has two fields: type and value.Type
        ///  should be one of type strings defined above.Value should correspond to the type. Example:
        ///
        ///      { "type" : "AccountId", "value" : "5ECcjykmdAQK71qHBCkEWpWkoMJY6NXvpdKy8UeMx16q5gFr"}
        ///
        ///   Information about Modules and State variables(with parameters and their types) is returned by getMetadata
        ///   method.
        /// </summary>
        /// <param name="jsonPrm"> JSON string that contains parameter and its type</param>
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="variable"> state variable (as in metadata for given module)</param>
        /// <returns> Storage size </returns>
        string GetStorageSize(string jsonPrm, string module, string variable);

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
