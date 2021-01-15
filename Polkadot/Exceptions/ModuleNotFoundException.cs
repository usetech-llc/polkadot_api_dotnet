using System;

namespace Polkadot.Exceptions
{
    public class ModuleNotFoundException : ArgumentException
    {
        public ModuleNotFoundException(string moduleName) : base($"There is no module {moduleName} in metadata.")
        {
        }
    }
}