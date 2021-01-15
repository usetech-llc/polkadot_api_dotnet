using Polkadot.BinarySerializer;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata
{
    using Extensions.Data;
    using Polkadot.Api;
    using Polkadot.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    public class Metadata
    {
        public int MetadataVersion { get; private set; }
        private readonly MetadataBase _metadata;

        public IMetadata RawMetadata => _metadata;
        
        public Metadata(MetadataBase metadata)
        {
            _metadata = metadata;
        }

        public T GetMetadata<T>() where T : MetadataBase
        {
            return _metadata as T;
        }

        public int GetMetadataVersion()
        {
            return _metadata.Version;
        }

        public FunctionCallArgV8[] GetModuleCallParamsByIds(int moduleIndex, int callIndex)
        {
            var md = _metadata as dynamic;
            int mi = 0;
            int i = 0;

            do
            {
                if (md.Module[i].Call != null)// && md.Module[i].Call[0].Name != null)
                {
                    mi++;
                }
                i++;
            }
            while (mi != moduleIndex);

            return md.Module[i].Call[callIndex].Args;
        }

        public Tuple<string, string> GetModuleCallNameByIds(int moduleIndex, int callIndex)
        {
            var md = _metadata as dynamic;
            int mi = 0;
            int i = 0;

            do
            {
                if (md.Module[i].Call != null)// && md.Module[i].Call[0].Name != null)
                {
                    mi++;
                }
                i++;
            }
            while (mi != moduleIndex);

            dynamic item = md.Module[i].Call[callIndex];
            return new Tuple<string, string>(md.Module[i].Name, item.Name);
        }

        public int GetModuleIndex(string moduleName, bool skipZeroCalls)
        {
            // Find the module index in metadata
            int moduleIndex = -1;
            int emptyCount = 0;

            if (IsKusamaBased())
            {
                return skipZeroCalls
                    ? _metadata.ModuleIndexSkipZeroCalls(moduleName)
                    : _metadata.ModuleIndex(moduleName);
            }

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
                    else if (!HasMethods(module.ind) && skipZeroCalls)
                    {
                        emptyCount++;
                    }
                }
            }

            return moduleIndex - emptyCount;
        }

        public bool HasMethods(int moduleIndex)
        {
            bool result = true;

            if (IsKusamaBased())
            {
                var md = _metadata as dynamic;
                result = md.Module[moduleIndex].Call != null;
            }

            if (_metadata.Version == 4)
            {
                var md = _metadata as MetadataV4;
                result = md.Module[moduleIndex].Call != null;
            }

            return result;
        }

        public int GetStorageMethodIndex(string moduleName, string funcName)
        {
            int methodIndex = -1;

            if (IsKusamaBased())
            {
                var module = _metadata.GetModule(moduleName);
                return module.GetStorageIndex(funcName);
            }

            if (_metadata.Version == 4)
            {
                var md = (MetadataV4)_metadata;
                var moduleIndex = _metadata.ModuleIndex(moduleName);
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

        public int GetCallMethodIndex(string moduleName, string funcName)
        {
            int methodIndex = -1;

            if (IsKusamaBased())
            {
                var module = _metadata.GetModule(moduleName);
                return module.GetCallIndex(funcName);
            }

            if (_metadata.Version == 4)
            {
                var md = _metadata as MetadataV4;
                var module = md.Module[_metadata.ModuleIndex(moduleName)];

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
            if (IsKusamaBased())
            {
                var md = _metadata as dynamic;
                return md.Module[moduleIndex].Storage.Items[varIndex].Type.Type == 0;
            }

            if (_metadata.Version == 4)
            {
                var md = _metadata as MetadataV4;
                return md.Module[moduleIndex].Storage[varIndex].Type.Type == 0;
            }

            throw new ApplicationException($"Module + State variable not found: {moduleIndex}:{varIndex}");
        }

        public BigInteger GetConst(string module, string constName)
        {
            BigInteger value = -1;

            if (IsKusamaBased())
            {
                var valueStr = _metadata
                    .GetModule(module)
                    .GetConstant(constName)
                    .GetValue();

                value = Converters.FromHex(valueStr, false);
            }

            return value;
        }

        public string GetAddrFromPublicKey(PublicKey pubKey)
        {
            return AddressUtils.GetAddrFromPublicKey(pubKey);
        }

        public PublicKey GetPublicKeyFromAddr(Address address)
        {
            return AddressUtils.GetPublicKeyFromAddr(address);
        }
    
        private bool IsKusamaBased()
        {
            return _metadata.Version >= 7;
        }
    }
}