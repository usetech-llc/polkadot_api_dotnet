using System;
using System.IO;
using System.Numerics;

namespace Polkadot.BinarySerializer.Converters
{
    public class U128Converter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] parameters)
        {
            var bigInteger = (BigInteger) value;
            for (int i = 0; i < 128 / 8; i++)
            {
                var @byte = (byte)(bigInteger & 0xff);
                stream.WriteByte(@byte);
                bigInteger >>= 8;
            }
        }

        public object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] parameters)
        {
            var bytes = new byte[128 / 8];
            stream.Read(bytes, 0, bytes.Length);

            return new BigInteger(bytes, isUnsigned: true);
        }
    }
}