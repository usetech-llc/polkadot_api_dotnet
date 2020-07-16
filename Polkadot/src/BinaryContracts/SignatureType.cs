namespace Polkadot.BinaryContracts
{
    public enum SignatureType : byte
    {
        /// <summary>
        /// An Ed25519 signature.
        /// </summary>
        Ed25519 = 0,
        /// <summary>
        /// An Sr25519 signature.
        /// </summary>
        Sr25519 = 1,
        /// <summary>
        /// An ECDSA/SECP256k1 signature.
        /// </summary>
        Ecdsa = 2
    }
}