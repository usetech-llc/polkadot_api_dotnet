using System;
using System.Buffers;
using System.IO;
using System.Text;

namespace Polkadot.BinarySerializer.Converters
{
    public class Utf8StringConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] param)
        {
            var strValue = (string) value;
            if (strValue == null)
            {
                var zero = Scale.EncodeCompactInteger(0).Bytes;
                stream.Write(zero, 0, zero.Length);
                return;
            }
            var bytes = Encoding.UTF8.GetBytes(strValue);
            var compactLength = Scale.EncodeCompactInteger(bytes.Length).Bytes;
            stream.Write(compactLength, 0, compactLength.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        public object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] param)
        {
            var length = (int) Scale.DecodeCompactInteger(stream).Value;
            var bytes = ArrayPool<byte>.Shared.Rent(length);
            stream.Read(bytes, 0, length);
            var str = Encoding.UTF8.GetString(bytes, 0, length);
            ArrayPool<byte>.Shared.Return(bytes);
            return str;
        }
    }
}