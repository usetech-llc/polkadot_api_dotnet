using sr25519_dotnet.lib.Models;
using System;

namespace sr25519_dotnet.lib
{
    public static class SR25519KeypairExtensions
    {
        /// <summary>
        /// Get the byte array representation of a SR25519Keypair object.
        /// </summary>
        /// <param name="keys">The SR25519Keypair object.</param>
        /// <returns>Byte array.</returns>
        public static byte[] GetBytes(this SR25519Keypair keys)
        {
            var bytes = new byte[96];

            Buffer.BlockCopy(keys.Secret, 0, bytes, 0, Constants.SR25519_SECRET_SIZE);
            Buffer.BlockCopy(keys.Public, 0,
                bytes, Constants.SR25519_SECRET_SIZE, Constants.SR25519_PUBLIC_SIZE);

            return bytes;
        }
    }
}
