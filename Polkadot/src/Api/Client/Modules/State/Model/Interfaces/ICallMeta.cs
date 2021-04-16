using System.Collections;
using System.Collections.Generic;

namespace Polkadot.DataStructs.Metadata.Interfaces
{
    public interface ICallMeta
    {
        string GetName();
        IReadOnlyList<ICallArgument> GetArguments();
    }
}