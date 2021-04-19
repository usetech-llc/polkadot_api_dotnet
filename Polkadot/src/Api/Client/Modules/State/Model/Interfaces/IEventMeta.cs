using System.Collections.Generic;

namespace Polkadot.Api.Client.Modules.State.Model.Interfaces
{
    public interface IEventMeta
    {
        string GetName();

        IReadOnlyList<string> GetArguments();
    }
}