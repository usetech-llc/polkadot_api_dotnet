using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Polkadot.DataStructs.Metadata.Interfaces;
using Polkadot.Utils;

namespace Polkadot.DataStructs.Metadata
{
    public abstract class MetadataBase: IMetadata
    {
        private readonly Lazy<IDictionary<string, IModuleMeta>> _moduleLookup;
        private readonly Lazy<IDictionary<string, int>> _moduleIndexLookup;
        private Lazy<IDictionary<string, int>> _moduleIndexSkipZeroCallsLookup;

        public MetadataBase()
        {
            _moduleLookup = new Lazy<IDictionary<string, IModuleMeta>>(() => 
                GetModules()
                    .ToDictionary(m => m.GetName(), StringComparer.OrdinalIgnoreCase),
                LazyThreadSafetyMode.ExecutionAndPublication);
            
            _moduleIndexLookup = new Lazy<IDictionary<string, int>>(() =>
                {
                    var dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    int index = 0;
                    foreach (var module in GetModules())
                    {
                        dictionary[module.GetName()] = index;
                        index++;
                    }

                    return dictionary;
                },
                LazyThreadSafetyMode.ExecutionAndPublication);
            
            _moduleIndexSkipZeroCallsLookup = new Lazy<IDictionary<string, int>>(() =>
                {
                    var dictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                    int index = 0;
                    foreach (var module in GetModules())
                    {
                        var calls = module.GetCalls();
                        if (calls != null && calls.Count > 0)
                        {
                            dictionary[module.GetName()] = index;
                            index++;
                        }
                    }

                    return dictionary;
                },
                LazyThreadSafetyMode.ExecutionAndPublication);
        }
        
        public int Version { get; protected set; }
        public abstract IReadOnlyList<IModuleMeta> GetModules();
        public IModuleMeta GetModule(string name)
        {
            return _moduleLookup.Value.TryGetOrDefault(name);
        }

        public int ModuleIndex(string moduleName)
        {
            return _moduleIndexLookup.Value[moduleName];
        }

        public int ModuleIndexSkipZeroCalls(string moduleName)
        {
            return _moduleIndexSkipZeroCallsLookup.Value[moduleName];
        }

        public abstract string[] GetExtrinsicExtension();
    }
}