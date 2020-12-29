using System.Text;
using System.Security.Cryptography;
using Schnorrkel.Keys;
using System;
using dotnetstandard_bip39;
using StrobeNet.Extensions;

namespace Mnemonic
{
    public static class MnemonicSubstrate
    {
        private const int ITERATIONS = 2048; // number of pbkdf2 iterations

        /// `entropy` should be a byte array from a correctly recovered and checksumed BIP39.
        ///
        /// This function accepts slices of different length for different word lengths:
        ///
        /// + 16 bytes for 12 words.
        /// + 20 bytes for 15 words.
        /// + 24 bytes for 18 words.
        /// + 28 bytes for 21 words.
        /// + 32 bytes for 24 words.
        ///
        /// Any other length will return an error.
        ///
        /// `password` is analog to BIP39 seed generation itself, with an empty string being default.
        public static byte[] SeedFromEntropy(byte[] entropy, string password)
        { 
            // Generate a salt
            byte[] salt = Encoding.ASCII.GetBytes("mnemonic" + password);

            // Generate the hash
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(entropy, salt, ITERATIONS, HashAlgorithmName.SHA512);
            return pbkdf2.GetBytes(64);
        }

        public static byte[] GenerateSecretKeyFromMnemonic(string phrase)
        {
            var bip = new BIP39();
            var mnemonic = bip.MnemonicToEntropy(phrase, BIP39Wordlist.English);

            return SeedFromEntropy(mnemonic.ToByteArray(), "").AsMemory().Slice(0, 32).ToArray();
        }

        public static KeyPair GeneratePairFromMnemonic(string phrase)
        {
            var secretBytes = GenerateSecretKeyFromMnemonic(phrase);
            var msk = new MiniSecret(secretBytes, ExpandMode.Ed25519);

            return new KeyPair(msk.ExpandToPublic(), msk.ExpandToSecret());
        }
    }
}
