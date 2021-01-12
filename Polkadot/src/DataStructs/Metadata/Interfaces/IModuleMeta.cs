using System.Collections.Generic;

namespace Polkadot.DataStructs.Metadata.Interfaces
{
    public interface IModuleMeta
    {
        IReadOnlyList<IConstantMeta> GetConstants();
        string GetName();
        IReadOnlyList<ICallMeta> GetCalls();
        IReadOnlyList<IEventMeta> GetEvents();
        IReadOnlyList<IStorage> GetStorages();

        IConstantMeta GetConstant(string constantName);
        int GetStorageIndex(string storageName);
    }
}