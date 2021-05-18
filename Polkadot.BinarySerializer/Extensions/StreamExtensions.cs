using System.IO;

namespace Polkadot.BinarySerializer.Extensions
{
    public static class StreamExtensions
    {
        public static byte ReadByteThrowIfStreamEnd(this Stream stream)
        {
            var read = stream.ReadByte();
            if (read == -1)
            {
                throw new EndOfStreamException();
            }

            return (byte) read;
        }

        public static bool TryReadByte(this Stream stream, out byte b)
        {
            b = 0;
            var read = stream.ReadByte();
            if (read == -1)
            {
                return false;
            }

            b = (byte) read;
            return true;
        }
    }
}