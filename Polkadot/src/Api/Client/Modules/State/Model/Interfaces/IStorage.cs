using OneOf;
using Polkadot.Api.Client.Modules.State.Model.StorageEntryType;

namespace Polkadot.Api.Client.Modules.State.Model.Interfaces
{
    public interface IStorage
    {
        string GetName();

        OneOf<Plain, Map, DoubleMap> GetStorageType();
    }
}