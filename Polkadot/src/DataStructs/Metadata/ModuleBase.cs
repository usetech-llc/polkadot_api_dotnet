using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Polkadot.DataStructs.Metadata.Interfaces;
using Polkadot.Utils;

namespace Polkadot.DataStructs.Metadata
{
    public abstract class ModuleBase : IModuleMeta
    {
        private readonly Lazy<IDictionary<string, IConstantMeta>> _constantLookup;
        
        public ModuleBase()
        {
            _constantLookup = new Lazy<IDictionary<string, IConstantMeta>>(() => 
                GetConstants()
                    .ToDictionary(c => c.GetName(), c => c, StringComparer.OrdinalIgnoreCase),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }
        
        public abstract IReadOnlyList<IConstantMeta> GetConstants();
        public abstract string GetName();
        public abstract IReadOnlyList<ICallMeta> GetCalls();
        public abstract IReadOnlyList<IEventMeta> GetEvents();
        public abstract IReadOnlyList<IStorage> GetStorages();

        public IConstantMeta GetConstant(string constantName)
        {
            return _constantLookup.Value.TryGetOrDefault(constantName);
        }

        public int GetStorageIndex(string storageName)
        {
            return GetStorages()
                .Select((storage, index) => (storage, index))
                .First(s => string.Equals(s.storage.GetName(), storageName, StringComparison.OrdinalIgnoreCase))
                .index;
        }
    }
}