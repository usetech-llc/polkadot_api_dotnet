using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata
{
    public abstract class ModuleBase : IModule
    {
        private Lazy<IDictionary<string, IConstant>> _constantLookup;
        
        public ModuleBase()
        {
            _constantLookup = new Lazy<IDictionary<string, IConstant>>(() => 
                GetConstants()
                    .ToDictionary(c => c.GetName(), c => c, StringComparer.OrdinalIgnoreCase),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }
        
        public abstract IEnumerable<IConstant> GetConstants();
        public abstract string GetName();

        public IDictionary<string, IConstant> ConstantLookup()
        {
            return _constantLookup.Value;
        }
    }
}