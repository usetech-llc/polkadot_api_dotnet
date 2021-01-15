using System;

namespace Polkadot.Exceptions
{
    public class StorageNotFoundException: ArgumentException
    {
        public StorageNotFoundException(string moduleName, string storageName) : base($"There is no storage {storageName} in module {moduleName}.")
        {
        }
    }
}