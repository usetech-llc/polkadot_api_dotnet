namespace Schnorrkel
{
    public class PublicKey
    {
        public byte[] Key { get; }

        public PublicKey(byte[] keyBytes)
        {
            Key = keyBytes;
        }
    }
}
