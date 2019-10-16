using System;

namespace sr25519_dotnet.lib.Exceptions
{
    public class SR25519KeypairException : Exception
    {
        public SR25519KeypairException(string message) :
            base(message)
        { }
    }
}
