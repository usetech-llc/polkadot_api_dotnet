using System.Collections.Generic;

namespace Polkadot.DataStructs.Metadata.Interfaces
{
    public interface IMetadata
    {
        IEnumerable<IModule> GetModules();

        IDictionary<string, IModule> ModuleLookup();

        string[] GetExtrinsicExtension();
    }
}