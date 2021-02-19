using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polkadot.Api.Hashers;
using Polkadot.Data;
using Polkadot.DataStructs;
using Polkadot.Exceptions;
using Polkadot.Utils;

namespace Polkadot.Api
{
    public class StorageApi: IStorageApi
    {
        private readonly Application _application;

        internal StorageApi(Application application)
        {
            _application = application;
        }

        public BlockHash GetBlockHash(GetBlockHashParams block)
        {
            return _application.GetBlockHash(block);
        }

        public T MakeRequest<T>(JObject request, int? timeout = null)
        {
            JObject response = _application.Request(request, timeout);

            return response["result"].ToObject<T>();
        }
        
        public string GetKeys(string moduleName, string storageName)
        {
            return GetKeys<object, object>(moduleName, storageName, null, null);
        }

        public string GetKeys<TKey>(string moduleName, string storageName, TKey key)
        {
            return GetKeys<TKey, object>(moduleName, storageName, key, null);
        }

        public string GetKeys<TKey1, TKey2>(string moduleName, string storageName, TKey1 key1, TKey2 key2)
        {
            var module = _application.GetMetadata(null).GetModule(moduleName);
            if (module == null)
            {
                throw new ModuleNotFoundException(moduleName);
            }
            var storage = module.GetStorage(storageName);
            if (storage == null)
            {
                throw new StorageNotFoundException(moduleName, storageName);
            }

            return storage
                .GetStorageType()
                .Match(
                    _ => 
                        PlainKey(moduleName)
                            .Concat(PlainKey(storageName))
                            .ToArray(),
                    map => 
                        PlainKey(moduleName)
                            .Concat(PlainKey(storageName))
                            .Concat(ComputeHash(key1, map.Hasher))
                            .ToArray(),
                    doubleMap => 
                        PlainKey(moduleName)
                            .Concat(PlainKey(storageName))
                            .Concat(ComputeHash(key1, doubleMap.Hasher))
                            .Concat(ComputeHash(key2, doubleMap.Key2Hasher))
                            .ToArray()
                ).ToPrefixedHexString();
        }

        private byte[] PlainKey<TKey1>(TKey1 key)
        {
            return ComputeHash(key, _application.PlainHasher);
        }

        private byte[] ComputeHash<TKey1>(TKey1 key, IHasher hasher)
        {
            if (key is IEnumerable enumerable && !(key is string))
            {
                return hasher.HashMultiple(enumerable.Cast<object>().Select(o => _application.Serializer.Serialize(o)));
            }

            return hasher.Hash(_application.Serializer.Serialize(key));
        }
        
         private JArray StorageParams(string childStorageKey, string storageKey, GetBlockHashParams block)
        {
            if (block != null)
            {
                var blockHash = GetBlockHash(block);
                return new JArray { childStorageKey, storageKey, blockHash.Hash };
            }

            return new JArray { childStorageKey, storageKey };
        }

        private TResult RequestMethod<TResult>(string childStorageKey, string storageKey, GetBlockHashParams block, string methodName)
        {
            var @params = StorageParams(childStorageKey, storageKey, block); 
            JObject query = new JObject { { "method", methodName }, { "params", @params } };
            return MakeRequest<TResult>(query);
        }
        
        private JArray StorageParams<TKey1, TKey2>(string module, string storageName, TKey1 key1, TKey2 key2, GetBlockHashParams block)
        {
            var keyHash = GetKeys(module, storageName, key1, key2);
            if (block != null)
            {
                var blockHash = GetBlockHash(block);
                return new JArray {keyHash, blockHash.Hash};
            }
            return new JArray {keyHash};
        }

        private TResult RequestMethod<TKey1, TKey2, TResult>(string module, string storageName, TKey1 key1, TKey2 key2,
            GetBlockHashParams block, string methodName)
        {
            var @params = StorageParams(module, storageName, key1, key2, block); 
            JObject query = new JObject { { "method", methodName }, { "params", @params } };
            return MakeRequest<TResult>(query);
        }
 
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
        public string GetStorage(string module, string storageName, GetBlockHashParams block = null)
        {
            return GetStorage<object, object>(module, storageName, null, null, block);
        }

        public string GetStorage(IEnumerable<string> rawStorage)
        {
            //  var @params = StorageParams(module, storageName, key1, key2, block);
            JArray paramsArr = new JArray(rawStorage);
            JObject query = new JObject { { "method", "state_getStorage" }, { "params", paramsArr } };
            return MakeRequest<string>(query);

          //  return GetStorage<object, object>(module, storageName, null, null, block);
        }

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
        public string GetStorage<TKey>(string module, string storageName, TKey key, GetBlockHashParams block = null)
        {
            return GetStorage<TKey, object>(module, storageName, key, null, block);
        }
        
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
        public string GetStorage<TKey1, TKey2>(string module, string storageName, TKey1 key1, TKey2 key2, GetBlockHashParams block = null)
        {
            return RequestMethod<TKey1, TKey2, string>(module, storageName, key1, key2, block, "state_getStorage");
        }

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
        /// <param name="storageName"> state variable (as in metadata for given module)</param>
        /// <param name="block"></param>
        /// <returns> Storage hash </returns>
        public string GetStorageHash(string module, string storageName, GetBlockHashParams block = null)
        {
            return GetStorageHash<object, object>(module, storageName, null, null, block);
        }

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
        /// <param name="storageName"> state variable (as in metadata for given module)</param>
        /// <param name="key">Key of map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="block"></param>
        /// <returns> Storage hash </returns>
        public string GetStorageHash<TKey>(string module, string storageName, TKey key, GetBlockHashParams block = null)
        {
            return GetStorageHash<TKey, object>(module, storageName, key, null, block);
        }

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
        /// <param name="storageName"> state variable (as in metadata for given module)</param>
        /// <param name="key1">First key of double map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="key2">Second key of double map storage. IEnumerable of keys if key is complex.</param>
        /// <param name="block"></param>
        /// <returns> Storage hash </returns>
        public string GetStorageHash<TKey1, TKey2>(string module, string storageName, TKey1 key1, TKey2 key2,
            GetBlockHashParams block = null)
        {
            return RequestMethod<TKey1, TKey2, string>(module, storageName, key1, key2, block, "state_getStorageHash");
        }

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
        public int GetStorageSize(string module, string storageName, GetBlockHashParams block = null)
        {
            return GetStorageSize<object, object>(module, storageName, null, null, block);
        }

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
        public int GetStorageSize<TKey>(string module, string storageName, TKey key, GetBlockHashParams block = null)
        {
            return GetStorageSize<TKey, object>(module, storageName, key, null, block);
        }

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
        public int GetStorageSize<TKey1, TKey2>(string module, string storageName, TKey1 key1, TKey2 key2,
            GetBlockHashParams block = null)
        {
            return RequestMethod<TKey1, TKey2, int>(module, storageName, key1, key2, block, "state_getStorageSize");
        }
        
        /// <summary>
        /// Calls storage_getChildKeys RPC method with given child storage key and storage key
        /// </summary>
        /// <param name="childStorageKey">string with 0x prefixed child storage key hex value</param>
        /// <param name="storageKey">string with 0x prefixed storage key hex value</param>
        /// <param name="block"></param>
        /// <returns>string response from RPC method</returns>
        public string GetChildKeys(string childStorageKey, string storageKey, GetBlockHashParams block = null)
        {
            return RequestMethod<string>(childStorageKey, storageKey, block, "state_getChildKeys");
        }

        /// <summary>
        /// Calls storage_getChildStorage RPC method with given child storage key and storage key
        /// </summary>
        /// <param name="childStorageKey"> string with 0x prefixed child storage key hex value </param>
        /// <param name="storageKey"> string with 0x prefixed storage key hex value </param>
        /// <param name="block"></param>
        /// <returns> string response from RPC method </returns>
        public string GetChildStorage(string childStorageKey, string storageKey, GetBlockHashParams block = null)
        {
            return RequestMethod<string>(childStorageKey, storageKey, block, "state_getChildStorage");
        }

        /// <summary>
        /// Calls storage_getChildStorageHash RPC method with given child storage key and storage key
        /// </summary>
        /// <param name="childStorageKey"> string with 0x prefixed child storage key hex value </param>
        /// <param name="storageKey"> string with 0x prefixed storage key hex value </param>
        /// <param name="block"></param>
        /// <returns> string response from RPC method </returns>
        public string GetChildStorageHash(string childStorageKey, string storageKey, GetBlockHashParams block = null)
        {
            return RequestMethod<string>(childStorageKey, storageKey, block, "state_getChildStorageHash");
        }

        /// <summary>
        /// Calls storage_getChildStorageSize RPC method with given child storage key and storage key
        /// </summary>
        /// <param name="childStorageKey">  string with 0x prefixed child storage key hex value </param>
        /// <param name="storageKey"> string with 0x prefixed storage key hex value </param>
        /// <param name="block"></param>
        /// <returns> int response from RPC method </returns>
        public int GetChildStorageSize(string childStorageKey, string storageKey, GetBlockHashParams block = null)
        {
            return RequestMethod<int>(childStorageKey, storageKey, block, "state_getChildStorageSize");
        }
 
        
        /// <summary>
        /// Calls state_queryStorage RPC method to get historical information about storage at a key
        /// </summary>
        /// <param name="key"> storage key to query </param>
        /// <param name="startHash"> hash of block to start with </param>
        /// <param name="stopHash"> hash of block to stop at </param>
        /// <param name="itemCount"> size of StorageItem elements for retrieve </param>
        /// <returns> array of StorageItem elements </returns>
        public IEnumerable<StorageItem> QueryStorage(string key, string startHash, string stopHash, int itemCount)
        {
            JObject query = new JObject { { "method", "state_queryStorage" },
                { "params", new JArray { new JArray(key), startHash, stopHash } } };
            var response = _application.Request(query, 30);

            int i = 0;
            dynamic values = response;

            while (i < itemCount && (values.Count > i))
            {
                var item = new StorageItem
                {
                    BlockHash = values[i]["block"].ToString(),
                    Key = values[i]["changes"][0][0].ToString(),
                    Value = values[i]["changes"][0][1].ToString()
                };
                yield return item;
                i++;
            }
        }
 
        public string SubscribeStorage(string key, Action<string> callback)
        {
            // Subscribe to websocket
            var subscribeQuery =
                new JObject { { "method", "state_subscribeStorage" }, { "params", new JArray { new JArray { key } } } };

            return _application.Subscribe(subscribeQuery, (json) =>
            {
                callback(json["result"]["changes"][0][1].ToString());
            });
        }
      
        public void UnsubscribeStorage(string id)
        {
            _application.RemoveSubscription(id, "state_unsubscribeStorage");
        }
    }
}