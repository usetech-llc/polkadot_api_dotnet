using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata
{
    public abstract class ModuleBase : IModuleMeta
    {
        private Lazy<IDictionary<string, IConstantMeta>> _constantLookup;
        
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

        public IDictionary<string, IConstantMeta> ConstantLookup()
        {
            return _constantLookup.Value;
        }
    }
}