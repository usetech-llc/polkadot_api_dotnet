using System.Collections.Generic;

namespace Polkadot.DataStructs.Metadata.Interfaces
{
    public interface IMetadata
    {
        IEnumerable<IModuleMeta> GetModules();

        IDictionary<string, IModuleMeta> ModuleLookup();

        string[] GetExtrinsicExtension();
    }
}