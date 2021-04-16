using System.Collections.Generic;

namespace Polkadot.DataStructs.Metadata.Interfaces
{
    public interface IEventMeta
    {
        string GetName();

        IReadOnlyList<string> GetArguments();
    }
}