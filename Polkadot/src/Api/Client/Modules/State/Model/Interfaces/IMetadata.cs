using System.Collections.Generic;

namespace Polkadot.DataStructs.Metadata.Interfaces
{
    public interface IMetadata
    {
        IReadOnlyList<IModuleMeta> GetModules();

        IModuleMeta GetModule(string name);

        int ModuleIndex(string moduleName);
        
        int ModuleIndexSkipZeroCalls(string moduleName);

        string[] GetExtrinsicExtension();
    }
}