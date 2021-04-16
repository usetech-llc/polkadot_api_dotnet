using System;

namespace Polkadot.Api.Client.Exceptions
{
    public class TransportException : Exception
    {
        public TransportException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}