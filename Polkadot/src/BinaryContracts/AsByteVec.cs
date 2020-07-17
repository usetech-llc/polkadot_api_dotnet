using System.IO;
using Polkadot.BinarySerializer;
using Polkadot.Utils;

namespace Polkadot.BinaryContracts
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
    public class AsByteVec<T> : AsByteVec, IBinarySerializable
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
            stream.Write(lengthBytes);
            
            stream.Write(valueBytes);
        }
    }
}