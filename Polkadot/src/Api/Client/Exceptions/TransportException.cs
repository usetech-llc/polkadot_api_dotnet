using System;
using System.Linq;
using System.Net.Sockets;
using Polkadot.Utils;

namespace Polkadot.Api.Client.Exceptions
{
    public class TransportException : Exception
    {
        public TransportException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public bool IsConnectionTimeout()
        {
            return InnerException.OfType<SocketException>().Any(s => s.SocketErrorCode == SocketError.TimedOut);
        }

        public bool IsAnyDisconnectedException()
        {
            return true;
        }
    }
}