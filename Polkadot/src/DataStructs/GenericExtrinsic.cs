namespace Polkadot.Api
{
    public enum ExtrinsicEra
    {
        IMMORTAL_ERA = 0, MORTAL_ERA = 1
    }

    public class Signature
    {
        public ushort Version { get; set; }
        public ushort[] SignerPublicKey { get; set; }
        public ushort Sr25519Signature { get; set; }
        public ushort Nonce { get; set; }
        public ExtrinsicEra Era { get; set; }
    }

    public class Extrinsic
    {
        public ulong Length { get; set; }
        public Signature MethodBytes { get; set; }
    }

    public class GenericMethod
    {
        public string MethodBytes { get; set; }
    }

    public class GenericExtrinsic
    {
        public GenericMethod Method { get; set; }
        public string SignerAddress { get; set; }
    }
}