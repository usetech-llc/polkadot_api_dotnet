namespace Polkadot.DataStructs.Metadata
{
    using Extensions.Data;
    using Polkadot.Api;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Metadata 
    {
        public int MetadataVersion { get; private set; }
        private MetadataV4 _MDV4 { get; set; }
        private readonly MetadataBase _metadata;

        public Metadata(MetadataBase metadata)
        {
            _metadata = metadata;
        }

        public T GetMetadata<T>() where T: MetadataBase
        {   
            return _metadata as T;
        }

        public int GetModuleIndex(string moduleName, bool skipZeroCalls)
        {
            // Find the module index in metadata
            int moduleIndex = -1;

            if (_metadata.Version == 4)
            {
                var md = _metadata as MetadataV4;
                foreach (var module in md.Module.Select((item, ind) => new { item, ind }))
                {
                    if (module.item.Name.Equals(moduleName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        moduleIndex = module.ind;
                        break;
                    }
                }
                return skipZeroCalls
                    ? moduleIndex - md.Module.Where(item => item.Name.Equals(string.Empty, StringComparison.InvariantCultureIgnoreCase)).Count()
                    : moduleIndex;
            }

            return moduleIndex;
        }

        public int GetStorageMethodIndex(int moduleIndex, string funcName)
        {
            int methodIndex = -1;

            if (_metadata.Version == 4)
            {
                var md = _metadata as MetadataV4;
                var module = md.Module.ElementAtOrDefault(moduleIndex);

                foreach (var storage in module.Storage.Select((item, ind) => new { item, ind }))
                {
                    if (storage.item.Name.Equals(funcName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        methodIndex = storage.ind;
                        break;
                    }
                }
            }
            return methodIndex;
        }

        public int GetCallMethodIndex(int moduleIndex, string funcName)
        {
            int methodIndex = -1;

            if (_metadata.Version == 4)
            {
                var md = _metadata as MetadataV4;
                var module = md.Module.ElementAtOrDefault(moduleIndex);

                foreach (var call in module.Call.Select((item, ind) => new { item, ind }))
                {
                    if (call.item.Name.Equals(funcName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        methodIndex = call.ind;
                        break;
                    }
                }
            }
            return methodIndex;
        }

        public bool IsStateVariablePlain(int moduleIndex, int varIndex)
        {
            if (_metadata.Version == 4)
            {
                var md = _metadata as MetadataV4;
                return md.Module[moduleIndex].Storage[varIndex].Type.Type == 0;
            }

            throw new ApplicationException($"Module + State variable not found: {moduleIndex}:{varIndex}");
        }

        public string GetPlainStorageKey(Hasher hasher, string prefix)
        {
            return GetStorageKey(hasher, Encoding.ASCII.GetBytes(prefix), prefix.Length);
        }

        public string GetAddressStorageKey(Hasher hasher, Address address, string prefix)
        {
            PublicKey pubk = GetPublicKeyFromAddr(address);
            var data = new List<byte>();

            data.AddRange(Encoding.ASCII.GetBytes(prefix));
            data.AddRange(pubk.Bytes);

            return GetStorageKey(hasher, data.ToArray(), Consts.PUBLIC_KEY_LENGTH + prefix.Length);
        }

        public PublicKey GetPublicKeyFromAddr(Address address)
        {
            var pubkByteList = new List<byte>();

            var bs58decoded = SimpleBase.Base58.Bitcoin.Decode(address.Symbols).ToArray();
            int len = bs58decoded.Length;

            if (len == 35)
            {
                // Check the address checksum
                // Add SS58RPE prefix, remove checksum (2 bytes)
                byte[] ssPrefixed = { 0x53, 0x53, 0x35, 0x38, 0x50, 0x52, 0x45 };
                pubkByteList.AddRange(ssPrefixed);
                pubkByteList.AddRange(bs58decoded.Take(Consts.PUBLIC_KEY_LENGTH + 1));

                var blake2bHashed = Blake2Core.Blake2B.ComputeHash(pubkByteList.ToArray(), new Blake2Core.Blake2BConfig { OutputSizeInBytes = 64, Key = null });
                if (bs58decoded[Consts.PUBLIC_KEY_LENGTH + 1] != blake2bHashed[0] ||
                    bs58decoded[Consts.PUBLIC_KEY_LENGTH + 2] != blake2bHashed[1])
                {
                    throw new ApplicationException("Address checksum is wrong.");
                }

                return new PublicKey { Bytes = bs58decoded.Skip(1).Take(Consts.PUBLIC_KEY_LENGTH).ToArray() };
            }

            throw new ApplicationException("Address checksum is wrong.");
        }

        public string GetStorageKey(Hasher hasher, byte[] data, int lenght)
        {
            // byte[] key = new byte[2 * Consts.STORAGE_KEY_BYTELENGTH + 3];
            string key = string.Empty;

            if (hasher == Hasher.XXHASH)
            {
                var xxhash1 = XXHash.XXH64(data, 0, lenght, 0);
                byte[] bytes1 = new byte[] {
                    (byte)(xxhash1 & 0xFF),
                    (byte)((xxhash1 & 0xFF00) >> 8),
                    (byte)((xxhash1 & 0xFF0000) >> 16),
                    (byte)((xxhash1 & 0xFF000000) >> 24),
                    (byte)((xxhash1 & 0xFF00000000) >> 32),
                    (byte)((xxhash1 & 0xFF0000000000) >> 40),
                    (byte)((xxhash1 & 0xFF000000000000) >> 48),
                    (byte)((xxhash1 & 0xFF00000000000000) >> 56)
                };

                var xxhash2 = XXHash.XXH64(data, 0, lenght, 1);
                byte[] bytes2 = new byte[] {
                    (byte)(xxhash2 & 0xFF),
                    (byte)((xxhash2 & 0xFF00) >> 8),
                    (byte)((xxhash2 & 0xFF0000) >> 16),
                    (byte)((xxhash2 & 0xFF000000) >> 24),
                    (byte)((xxhash2 & 0xFF00000000) >> 32),
                    (byte)((xxhash2 & 0xFF0000000000) >> 40),
                    (byte)((xxhash2 & 0xFF000000000000) >> 48),
                    (byte)((xxhash2 & 0xFF00000000000000) >> 56)
                };

                foreach (var bt in bytes1)
                {
                    key += bt.ToString("X2");
                }

                foreach (var bt in bytes2)
                {
                    key += bt.ToString("X2");
                }
            }
            else if (hasher == Hasher.BLAKE2)
            {
                var hash = Blake2Core.Blake2B.ComputeHash(data, 0, lenght);

                foreach(var bt in hash)
                {
                    key += bt.ToString("X2");
                }
            }

            return $"0x{key}";
        }

        public string GetMappedStorageKey(Hasher hasher, KeyValuePair<string, string> param, string prefix)
        {
            if (param.Key == Consts.STORAGE_TYPE_ADDRESS)
            {
                return GetAddressStorageKey(hasher, new Address { Symbols = param.Value }, prefix);
            }
            else if (param.Key == Consts.STORAGE_TYPE_BLOCK_NUMBER || param.Key == Consts.STORAGE_TYPE_U32 ||
                param.Key == Consts.STORAGE_TYPE_ACCOUNT_INDEX || param.Key == Consts.STORAGE_TYPE_PROPOSAL_INDEX ||
                param.Key == Consts.STORAGE_TYPE_REFERENDUM_INDEX || param.Key == Consts.STORAGE_TYPE_PARACHAIN_ID)
            {
                var key = $"{prefix} {param.Value}";
                return GetStorageKey(hasher, Encoding.ASCII.GetBytes(key), key.Length);
            }
            else if (param.Key == Consts.STORAGE_TYPE_HASH)
            {
                var key = $"{prefix} {param.Value}";
                return GetStorageKey(hasher, Encoding.ASCII.GetBytes(key), key.Length);
            }

            throw new ApplicationException($"Storage key with type {param.Key} is not defined");
        }

        public Hasher GetFuncHasher(string moduleName, string funcName)
        {
            Hasher hasher = Hasher.XXHASH;

            // Find the module index in metadata
            int moduleIndex = GetModuleIndex(moduleName, false);

            // Find function by name in module and get its hasher
            uint hasherEnum = 0;
            if (moduleIndex >= 0)
            {
                int methodIndex = GetStorageMethodIndex(moduleIndex, funcName);
                if (methodIndex > 0)
                {
                    if (_metadata.Version == 4)
                    {
                        var md = _metadata as MetadataV4;
                        hasherEnum = md.Module[moduleIndex].Storage[methodIndex].Type.Hasher;
                    }
                }
            }

            // Parse hasher name
            if (hasherEnum == 1)
            {
                hasher = Hasher.BLAKE2;
            }

            return hasher;
        }
    }
}