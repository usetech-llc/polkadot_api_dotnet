using System.Collections.Generic;

namespace Polkadot.DataStructs.Metadata.Interfaces
{
    public interface IModuleMeta
    {
        IReadOnlyList<IConstantMeta> GetConstants();
        string GetName();
        IReadOnlyList<ICallMeta> GetCalls();
        IReadOnlyList<IEventMeta> GetEvents();

        IDictionary<string, IConstantMeta> ConstantLookup();
    }
}