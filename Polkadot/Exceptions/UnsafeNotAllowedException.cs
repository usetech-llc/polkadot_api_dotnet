using System;

namespace Polkadot.Exceptions
{
    public class UnsafeNotAllowedException : Exception
    {
        public UnsafeNotAllowedException()
        {
        }

        public UnsafeNotAllowedException(string methodName) : base($"Failed to call {methodName}, this might be because node doesn't allow unsafe method calls.")
        {
        }

        public UnsafeNotAllowedException(string methodName, Exception innerException) : base($"Failed to call {methodName}, this might be because node doesn't allow unsafe method calls.", innerException)
        {
        }
    }
}