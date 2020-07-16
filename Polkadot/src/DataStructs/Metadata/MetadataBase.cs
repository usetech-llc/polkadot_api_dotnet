using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata
{
    public abstract class MetadataBase: IMetadata
    {
        private Lazy<IDictionary<string, IModule>> _moduleLookup;

        public MetadataBase()
        {
            _moduleLookup = new Lazy<IDictionary<string, IModule>>(() => 
                GetModules()
                    .ToDictionary(m => m.GetName(), m => m, StringComparer.OrdinalIgnoreCase),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }
        
        public int Version { get; protected set; }
        public abstract IEnumerable<IModule> GetModules();
        public IDictionary<string, IModule> ModuleLookup()
        {
            return _moduleLookup.Value;
        }

        public abstract string[] GetExtrinsicExtension();
    }
}