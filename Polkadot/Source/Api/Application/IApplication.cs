namespace Polkadot.Api
{
    using System;
    using System.Threading.Tasks;
    using Polkadot.Data;
    using Polkadot.DataStructs.Metadata;

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

        /// <summary>
        ///  Retreives the runtime metadata for specific block
        /// </summary>
        /// <param name="param"> struct with blockHash 64 diget number in hex format </param>
        /// <returns> Metadata struct with result </returns>
        MetadataBase GetMetadata(GetMetadataParams param);

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
        int GetStorageSize(string jsonPrm, string module, string variable);

        /// <summary>
        /// Calls storage_getChildKeys RPC method with given child storage key and storage key
        /// </summary>
        /// <param name="childStorageKey">string with 0x prefixed child storage key hex value</param>
        /// <param name="storageKey">string with 0x prefixed storage key hex value</param>
        /// <returns>string response from RPC method</returns>
        string GetChildKeys(string childStorageKey, string storageKey);

        /// <summary>
        /// Calls storage_getChildStorage RPC method with given child storage key and storage key
        /// </summary>
        /// <param name="childStorageKey"> string with 0x prefixed child storage key hex value </param>
        /// <param name="storageKey"> string with 0x prefixed storage key hex value </param>
        /// <returns> string response from RPC method </returns>
        string GetChildStorage(string childStorageKey, string storageKey);

        /// <summary>
        /// Calls storage_getChildStorageHash RPC method with given child storage key and storage key
        /// </summary>
        /// <param name="childStorageKey"> string with 0x prefixed child storage key hex value </param>
        /// <param name="storageKey"> string with 0x prefixed storage key hex value </param>
        /// <returns> string response from RPC method </returns>
        string GetChildStorageHash(string childStorageKey, string storageKey);

        /// <summary>
        /// Calls storage_getChildStorageSize RPC method with given child storage key and storage key
        /// </summary>
        /// <param name="childStorageKey">  string with 0x prefixed child storage key hex value </param>
        /// <param name="storageKey"> string with 0x prefixed storage key hex value </param>
        /// <returns> int response from RPC method </returns>
        int GetChildStorageSize(string childStorageKey, string storageKey);

        /// <summary>
        /// Calls state_queryStorage RPC method to get historical information about storage at a key
        /// </summary>
        /// <param name="key"> storage key to query </param>
        /// <param name="startHash"> hash of block to start with </param>
        /// <param name="stopHash"> hash of block to stop at </param>
        /// <param name="itemCount"> size of StorageItem elements for retrieve </param>
        /// <returns> array of StorageItem elements </returns>
        StorageItem[] QueryStorage(string key, string startHash, string stopHash, int itemCount);

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
