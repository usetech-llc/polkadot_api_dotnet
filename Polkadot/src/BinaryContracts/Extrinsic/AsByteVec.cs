using System.IO;
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts.Extrinsic
{
    public class AsByteVec
    {
        public static AsByteVec<T> FromValue<T>(T value)
        {
            return new AsByteVec<T>(value);
        }
    }
    
    /// <summary>
    /// Objects like Extrinsics are also byte arrays with compact length prefixed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsByteVec<T> : AsByteVec, IBinarySerializable, IBinaryDeserializable
    {
        public T Value;

        public AsByteVec()
        {
        }

        public AsByteVec(T value)
        {
            Value = value;
        }

        public void Serialize(Stream stream, IBinarySerializer serializer)
        {
            var valueBytes = serializer.Serialize(Value);
            
            var lengthBytes = Scale.EncodeCompactInteger(valueBytes.Length).Bytes;
            stream.Write(lengthBytes, 0, lengthBytes.Length);
            
            stream.Write(valueBytes, 0, 0);
        }

        public object Deserialize(Stream stream, IBinarySerializer serializer)
        {
            Scale.DecodeCompactInteger(stream);
            return FromValue(serializer.Deserialize<T>(stream));
        }
    }
}