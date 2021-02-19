using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Polkadot.Data;
using Polkadot.DataStructs;

namespace Polkadot.Api
{
    public interface IStorageApi
    {
        BlockHash GetBlockHash(GetBlockHashParams block);
        
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
        /// <param name="moduleName">Module (as in metadata)</param>
        /// <param name="storageName">Storage name.</param>
        /// <returns> Storage key </returns>
        string GetKeys(string moduleName, string storageName);
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
        /// <param name="moduleName">Module (as in metadata)</param>
        /// <param name="storageName">Storage name.</param>
        /// <param name="key">Key of map storage. IEnumerable of keys if key is complex.</param>
        /// <returns> Storage key </returns>
        string GetKeys<TKey>(string moduleName, string storageName, TKey key);
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
        /// <param name="moduleName">Module (as in metadata)</param>
        /// <param name="storageName">Storage name.</param>
        /// <param name="key1">First key of double map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="key2">Second key of double map storage. IEnumerable of keys if key is complex.</param>
        /// <returns> Storage key </returns>
        string GetKeys<TKey1, TKey2>(string moduleName, string storageName, TKey1 key1, TKey2 key2);
        

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
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="storageName"> state variable (as in metadata for given module)</param>
        /// <param name="block"></param>
        /// <returns> Storage content </returns>
        string GetStorage(string module, string storageName, GetBlockHashParams block = null);

        string GetStorage(IEnumerable<string> rawStorage);

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
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="storageName"> state variable (as in metadata for given module)</param>
        /// <param name="key">Key of map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="block"></param>
        /// <returns> Storage content </returns>
        string GetStorage<TKey>(string module, string storageName, TKey key, GetBlockHashParams block = null);

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
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="storageName"> state variable (as in metadata for given module)</param>
        /// <param name="key1">First key of double map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="key2">Second key of double map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="block"></param>
        /// <returns> Storage content </returns>
        string GetStorage<TKey1, TKey2>(string module, string storageName, TKey1 key1, TKey2 key2, GetBlockHashParams block = null);

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
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="variable"> state variable (as in metadata for given module)</param>
        /// <param name="block"></param>
        /// <returns> Storage hash </returns>
        string GetStorageHash(string module, string variable, GetBlockHashParams block = null);

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
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="variable"> state variable (as in metadata for given module)</param>
        /// <param name="key">Key of map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="block"></param>
        /// <returns> Storage hash </returns>
        string GetStorageHash<TKey>(string module, string variable, TKey key, GetBlockHashParams block = null);

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
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="variable"> state variable (as in metadata for given module)</param>
        /// <param name="key1">First key of double map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="key2">Second key of double map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="block"></param>
        /// <returns> Storage hash </returns>
        string GetStorageHash<TKey1, TKey2>(string module, string variable, TKey1 key1, TKey2 key2, GetBlockHashParams block = null);

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
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="storageName"> state variable (as in metadata for given module)</param>
        /// <param name="block"></param>
        /// <returns> Storage size </returns>
        int GetStorageSize(string module, string storageName, GetBlockHashParams block = null);

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
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="storageName"> state variable (as in metadata for given module)</param>
        /// <param name="key">Key of map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="block"></param>
        /// <returns> Storage size </returns>
        int GetStorageSize<TKey>(string module, string storageName, TKey key, GetBlockHashParams block = null);

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
        /// <param name="module"> module (as in metadata)</param>
        /// <param name="storageName"> state variable (as in metadata for given module)</param>
        /// <param name="key1">First key of double map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="key2">Second key of double map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="block"></param>
        /// <returns> Storage size </returns>
        int GetStorageSize<TKey1, TKey2>(string module, string storageName, TKey1 key1, TKey2 key2, GetBlockHashParams block = null);


        /// <summary>
        /// Calls storage_getChildKeys RPC method with given child storage key and storage key
        /// </summary>
        /// <param name="childStorageKey">string with 0x prefixed child storage key hex value</param>
        /// <param name="storageKey">string with 0x prefixed storage key hex value</param>
        /// <param name="block"></param>
        /// <returns>string response from RPC method</returns>
        string GetChildKeys(string childStorageKey, string storageKey, GetBlockHashParams block = null);

        /// <summary>
        /// Calls storage_getChildStorage RPC method with given child storage key and storage key
        /// </summary>
        /// <param name="childStorageKey"> string with 0x prefixed child storage key hex value </param>
        /// <param name="storageKey"> string with 0x prefixed storage key hex value </param>
        /// <param name="block"></param>
        /// <returns> string response from RPC method </returns>
        string GetChildStorage(string childStorageKey, string storageKey, GetBlockHashParams block = null);

        /// <summary>
        /// Calls storage_getChildStorageHash RPC method with given child storage key and storage key
        /// </summary>
        /// <param name="childStorageKey"> string with 0x prefixed child storage key hex value </param>
        /// <param name="storageKey"> string with 0x prefixed storage key hex value </param>
        /// <param name="block"></param>
        /// <returns> string response from RPC method </returns>
        string GetChildStorageHash(string childStorageKey, string storageKey, GetBlockHashParams block = null);

        /// <summary>
        /// Calls storage_getChildStorageSize RPC method with given child storage key and storage key
        /// </summary>
        /// <param name="childStorageKey">  string with 0x prefixed child storage key hex value </param>
        /// <param name="storageKey"> string with 0x prefixed storage key hex value </param>
        /// <param name="block"></param>
        /// <returns> int response from RPC method </returns>
        int GetChildStorageSize(string childStorageKey, string storageKey, GetBlockHashParams block = null);

        /// <summary>
        /// Calls state_queryStorage RPC method to get historical information about storage at a key
        /// </summary>
        /// <param name="key"> storage key to query </param>
        /// <param name="startHash"> hash of block to start with </param>
        /// <param name="stopHash"> hash of block to stop at </param>
        /// <param name="itemCount"> size of StorageItem elements for retrieve </param>
        /// <returns> array of StorageItem elements </returns>
        IEnumerable<StorageItem> QueryStorage(string key, string startHash, string stopHash, int itemCount);

        /// <summary>
        /// Subscribe to most recent value updates for a given storage key. Only one subscription at a time per address is
        ///     allowed. If a subscription already exists for the same storage key, old subscription will be discarded and
        ///     replaced with the new one. Until unsubscribeStorage method is called with the same storage key, the API will be
        ///     receiving updates and forwarding them to subscribed object/function. Only unsubscribeStorage will physically
        ///     unsubscribe from WebSocket endpoint updates.
        /// </summary>
        /// <param name="key"> storage key to receive updates for (e.g. "0x66F795B8D457430EDDA717C3CBA459B9") </param>
        /// <param name="callback"> expression that will received </param>
        /// <returns> Subscription id </returns>
        string SubscribeStorage(string key, Action<string> callback);

        /// <summary>
        /// Unsubscribe from WebSocket endpoint and stop receiving updates for address balance.
        /// </summary>
        /// <param name="id"> Subscription id </param>
        void UnsubscribeStorage(string id);
       
    }
}