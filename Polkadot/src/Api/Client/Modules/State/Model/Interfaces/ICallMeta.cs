using System.Collections.Generic;

namespace Polkadot.Api.Client.Modules.State.Model.Interfaces
{
    public interface ICallMeta
    {
        string GetName();
        IReadOnlyList<ICallArgument> GetArguments();
    }
}