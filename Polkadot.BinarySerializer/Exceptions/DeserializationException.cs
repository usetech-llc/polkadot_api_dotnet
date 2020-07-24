using System;
using System.Runtime.Serialization;

namespace Polkadot.BinarySerializer
{
    public class DeserializationException : Exception
    {
        public DeserializationException()
        {
        }

        protected DeserializationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DeserializationException(string message) : base(message)
        {
        }

        public DeserializationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}