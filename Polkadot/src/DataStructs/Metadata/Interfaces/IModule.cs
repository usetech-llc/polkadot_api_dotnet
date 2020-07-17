using System.Collections.Generic;

namespace Polkadot.DataStructs.Metadata.Interfaces
{
    public interface IModule
    {
        IEnumerable<IConstant> GetConstants();
        string GetName();

        IDictionary<string, IConstant> ConstantLookup();
    }
}