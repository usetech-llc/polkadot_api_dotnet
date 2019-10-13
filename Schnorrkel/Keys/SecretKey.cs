namespace Schnorrkel
{
    using Schnorrkel.Scalars;
    using System;

    public struct SecretKey
    {
        /// Actual public key represented as a scalar.
        public Scalar key;
        /// Seed for deriving the nonces used in signing.
        ///
        /// We require this be random and secret or else key compromise attacks will ensue.
        /// Any modificaiton here may dirupt some non-public key derivation techniques.
        public byte[] nonce; //[u8; 32],

        public static SecretKey FromBytes(byte[] data)
        {
            if (data.Length != Consts.SIGNATURE_LENGTH)
                throw new Exception("SecretKey - SignatureError::BytesLengthError");

            // var key = data.AsMemory().Slice(0, 32).ToArray();
            return new SecretKey
            {
                key = new Scalar { ScalarBytes = data.AsMemory().Slice(0, 32).ToArray() },
                nonce = data.AsMemory().Slice(32, 32).ToArray()
            };
        }
    }
}
