using System;
using System.Runtime.Serialization;

namespace Polkadot.BinarySerializer
{
    public class NotAllDataUsedException : Exception
    {
        public NotAllDataUsedException()
        {
        }

        protected NotAllDataUsedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NotAllDataUsedException(string message) : base(message)
        {
        }

        public NotAllDataUsedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}