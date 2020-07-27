using System;
using System.Runtime.Serialization;

namespace Polkadot.BinarySerializer
{
    public class SerializationException : Exception
    {
        public SerializationException()
        {
        }

        protected SerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SerializationException(string message) : base(message)
        {
        }

        public SerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}