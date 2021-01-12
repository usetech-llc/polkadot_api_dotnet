using OneOf;
using Polkadot.DataStructs.Metadata.BinaryContracts.StorageEntryType;

namespace Polkadot.DataStructs.Metadata.Interfaces
{
    public interface IStorage
    {
        string GetName();

        OneOf<Plain, Map, DoubleMap> GetStorageType();
    }
}