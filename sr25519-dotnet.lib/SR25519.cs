using sr25519_dotnet.lib.Models;
using System;
using sr25519_dotnet.lib.Interop;
using System.Text;
using sr25519_dotnet.lib.Exceptions;

namespace sr25519_dotnet.lib
{
    /// <summary>
    /// SR25519 Operations.
    /// </summary>
    public class SR25519
    {
        /// <summary>
        /// Generates a SR25519 keypair from a hex seed string.
        /// </summary>
        /// <param name="seedHex">Seed string as hex.</param>
        /// <returns>SR25519Keypair</returns>
        public static SR25519Keypair GenerateKeypairFromSeed(string seedHex)
        {
            byte[] seedBytes = Utils.HexStringToByteArray(seedHex);

            var keys = new byte[Constants.SR25519_KEYPAIR_SIZE];
            Bindings.KeypairFromSeed(keys, seedBytes);

            return new SR25519Keypair(keys);
        }

        /// <summary>
        /// Hard derive a new keypair from an existing keypair.
        /// </summary>
        /// <param name="keypair">Input keypair.</param>
        /// <param name="chainCode">Chain code as hex string.</param>
        /// <returns>SR25519Keypair</returns>
        public static SR25519Keypair HardDeriveKeypair(SR25519Keypair keypair, string chainCodeHex)
        {
            byte[] chainCodeBytes = Utils.HexStringToByteArray(chainCodeHex);

            if (chainCodeBytes.Length != Constants.SR25519_CHAINCODE_SIZE)
            {
                throw new SR25519KeypairException(StringConstants.BadChaincodeSizeMessage);
            }

            var bytes = keypair.GetBytes();
            var derived = new byte[Constants.SR25519_KEYPAIR_SIZE];
            Bindings.DeriveKeypairHard(derived, bytes, chainCodeBytes);

            return new SR25519Keypair(derived);
        }

        /// <summary>
        /// Soft derive a new keypair from an existing keypair.
        /// </summary>
        /// <param name="keypair">Input keypair.</param>
        /// <param name="chainCodeHex">Chain code as hex string.</param>
        /// <returns>SR25519Keypair</returns>
        public static SR25519Keypair SoftDeriveKeypair(SR25519Keypair keypair, string chainCodeHex)
        {
            byte[] chainCodeBytes = Utils.HexStringToByteArray(chainCodeHex);

            if (chainCodeBytes.Length != Constants.SR25519_CHAINCODE_SIZE)
            {
                throw new SR25519KeypairException(StringConstants.BadChaincodeSizeMessage);
            }

            var bytes = keypair.GetBytes();
            var derived = new byte[Constants.SR25519_KEYPAIR_SIZE];
            Bindings.DeriveKeypairSoft(derived, bytes, chainCodeBytes);

            return new SR25519Keypair(derived);
        }

        /// <summary>
        /// Perform a derivation on a publicKey.
        /// </summary>
        /// <param name="publicKeyHex">Input public key as hex string.</param>
        /// <param name="chainCodeHex">Chain code as hex string.</param>
        /// <returns></returns>
        public static byte[] SoftDerivePublicKey(string publicKeyHex, string chainCodeHex)
        {
            var bytes = Utils.HexStringToByteArray(publicKeyHex);
            return SoftDerivePublicKey(bytes, chainCodeHex);
        }

        /// <summary>
        /// Perform a derivation on a publicKey.
        /// </summary>
        /// <param name="publicKey">Input public key as byte[].</param>
        /// <param name="chainCodeHex">Chain code as hex string.</param>
        /// <returns>byte[] public key</returns>
        public static byte[] SoftDerivePublicKey(byte[] publicKey, string chainCodeHex)
        {
            byte[] chainCodeBytes = Utils.HexStringToByteArray(chainCodeHex);

            if (chainCodeBytes?.Length != Constants.SR25519_CHAINCODE_SIZE)
            {
                throw new SR25519KeypairException(StringConstants.BadChaincodeSizeMessage);
            }

            if (publicKey?.Length != Constants.SR25519_PUBLIC_SIZE)
            {
                throw new SR25519KeypairException(StringConstants.BadKeySizeMessage);
            }

            var derived = new byte[Constants.SR25519_PUBLIC_SIZE];
            Bindings.DerivePublicSoft(derived, publicKey, chainCodeBytes);

            return derived;
        }

        /// <summary>
        /// Signs a message and returns the signature.
        /// </summary>
        /// <param name="message">The message to sign.</param>
        /// <param name="keypair">The keypair for signing.</param>
        /// <returns>Signature as byte[]</returns>
        public static byte[] Sign(string message, SR25519Keypair keypair)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var signature = new byte[Constants.SR25519_SIGNATURE_SIZE];

            Bindings.Sign(
                signature, keypair.Public, 
                keypair.Secret, bytes, Convert.ToUInt64(bytes.Length));

            return signature;
        }

        /// <summary>
        /// Signs a message and returns the signature.
        /// </summary>
        /// <param name="message">The message to sign.</param>
        /// <param name="keypair">The keypair for signing.</param>
        /// <returns>Signature as byte[]</returns>
        public static byte[] Sign(byte[] message, ulong messageLen, SR25519Keypair keypair)
        {
            var signature = new byte[Constants.SR25519_SIGNATURE_SIZE];

            Bindings.Sign(
                signature, keypair.Public,
                keypair.Secret, message, messageLen);

            return signature;
        }

        /// <summary>
        /// Verify the signature of a signed message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="signature">The message signature.</param>
        /// <param name="publicKey">The public (verification) key.</param>
        /// <returns>True/False if the verification passed or failed.</returns>
        public static bool Verify(string message, byte[] signature, 
            byte[] publicKey)
        {
            bool result;
            try
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                result = Bindings.Verify(
                    signature, bytes, Convert.ToUInt64(bytes.Length), 
                    publicKey);
            }
            catch (Exception)
            {
                return false;
            }

            return result;
        }
    }
}
