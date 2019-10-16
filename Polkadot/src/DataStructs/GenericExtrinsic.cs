namespace Polkadot.Api
{
    using Polkadot.DataStructs;
    using System.Numerics;

    public enum ExtrinsicEra
    {
        IMMORTAL_ERA = 0, MORTAL_ERA = 1
    }

    public class Signature
    {
        public byte Version { get; set; }
        public byte[] SignerPublicKey { get; set; }
        public byte[] Sr25519Signature { get; set; }
        public BigInteger Nonce { get; set; }
        public ExtrinsicEra Era { get; set; }
    }

    public class Extrinsic
    {
        public ulong Length { get; set; }
        public Signature Signature { get; set; }

        public Extrinsic()
        {
            Signature = new Signature();
        }
    }

    public class GenericMethod : Method
    {
        public string MethodBytes { get; set; }
    }

    public class GenericExtrinsic : Extrinsic
    {
        public GenericMethod Method { get; set; }
        public string SignerAddress { get; set; }

        public GenericExtrinsic()
        {
            Method = new GenericMethod();
        }
    }
}