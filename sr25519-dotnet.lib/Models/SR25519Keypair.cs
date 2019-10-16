using sr25519_dotnet.lib.Exceptions;
using System;

namespace sr25519_dotnet.lib.Models
{
    /// <summary>
    /// The SR25519 Keypair
    /// </summary>
    public class SR25519Keypair
    {
        /// <summary>
        /// The byte array representation of public key.
        /// </summary>
        public byte[] Public { get; private set; }

        /// <summary>
        /// Get the public key in hex string format.
        /// </summary>
        public string PublicHex
        {
            get
            {
                return Public?.Length == Constants.SR25519_PUBLIC_SIZE ? 
                    Utils.ByteArrayToHexString(this.Public) : null;
            }
        }

        /// <summary>
        /// The byte array representation of secret/private key.
        /// </summary>
        public byte[] Secret { get; private set; }

        /// <summary>
        /// Create a keypair from public and private key byte arrays.
        /// </summary>
        /// <param name="public_key"></param>
        /// <param name="secret_key"></param>
        public SR25519Keypair(byte[] public_key, byte[] secret_key)
        {
            if (public_key.Length != Constants.SR25519_PUBLIC_SIZE &&
                secret_key.Length != Constants.SR25519_SECRET_SIZE)
            {
                throw new SR25519KeypairException(StringConstants.BadKeySizeMessage);
            }

            this.Secret = new byte[Constants.SR25519_SECRET_SIZE];
            this.Public = new byte[Constants.SR25519_PUBLIC_SIZE];
            public_key.CopyTo(this.Public, 0);
            secret_key.CopyTo(this.Secret, 0);
        }

        /// <summary>
        /// Create a keypair object from its byte[] representation.
        /// </summary>
        /// <param name="keypair"></param>
        public SR25519Keypair(byte[] keypair)
        {
            if (keypair.Length != Constants.SR25519_KEYPAIR_SIZE)
            {
                throw new SR25519KeypairException(StringConstants.BadKeySizeMessage);
            }

            this.Secret = new byte[Constants.SR25519_SECRET_SIZE];
            this.Public = new byte[Constants.SR25519_PUBLIC_SIZE];
            Buffer.BlockCopy(keypair, 0, this.Secret, 0, Constants.SR25519_SECRET_SIZE);
            Buffer.BlockCopy(keypair, Constants.SR25519_SECRET_SIZE,
                this.Public, 0, Constants.SR25519_PUBLIC_SIZE);
        }
    }
}
