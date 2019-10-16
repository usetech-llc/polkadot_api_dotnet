namespace sr25519_dotnet.lib
{
    public static class Constants
    {
        /// <summary>
        /// Size of input seed in bytes.
        /// </summary>
        public const int SR25519_SEED_SIZE = 32;

        /// <summary>
        /// Size of SR25519 private key, which consists of: 32 bytes key + 32 bytes nonce
        /// </summary>
        public const int SR25519_SECRET_SIZE = 64;

        /// <summary>
        /// Size of SR25519 public key in bytes
        /// </summary>
        public const int SR25519_PUBLIC_SIZE = 32;

        /// <summary>
        /// Size of SR25519 signature in bytes
        /// </summary>
        public const int SR25519_SIGNATURE_SIZE = 64;

        /// <summary>
        /// Size of SR25519 keypair: 32 bytes private key + 32 bytes nonce + 32 bytes public key
        /// </summary>
        public const int SR25519_KEYPAIR_SIZE = 96;

        /// <summary>
        /// Size of chaincode in bytes
        /// </summary>
        public const int SR25519_CHAINCODE_SIZE = 32;
    }
}
