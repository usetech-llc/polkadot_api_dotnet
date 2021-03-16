using System.IO;
using System.Numerics;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using PublicKey = Polkadot.DataStructs.PublicKey;

namespace Polkadot.BinaryContracts.Calls.Contracts
{
    public partial class CallCall : IBinarySerializable, IBinaryDeserializable, IExtrinsicCall
    {
        public IContractCallParameter Parameters { get; set; }
        
        public void Serialize(Stream stream, IBinarySerializer serializer)
        {
            serializer.Serialize(Dest, stream);
            var bigIntegerConverter = (CompactBigIntegerConverter) serializer.GetConverter(typeof(CompactBigIntegerConverter));
            bigIntegerConverter.Serialize(stream, Value, serializer, null);
            bigIntegerConverter.Serialize(stream, GasLimit, serializer, null);
            var arrayConverter = serializer.GetConverter(typeof(PrefixedArrayConverter));
            arrayConverter.Serialize(stream, Data, serializer, null);
            
        }

        public object Deserialize(Stream stream, IBinarySerializer serializer)
        {
            var dest = serializer.Deserialize<PublicKey>(stream);
            var bigIntegerConverter = (CompactBigIntegerConverter) serializer.GetConverter(typeof(CompactBigIntegerConverter));
            var value = (BigInteger) bigIntegerConverter.Deserialize(typeof(BigInteger), stream, serializer, null);
            var gasLimit = (BigInteger) bigIntegerConverter.Deserialize(typeof(BigInteger), stream, serializer, null);
            var arrayConverter = serializer.GetConverter(typeof(PrefixedArrayConverter));
            var data = (byte[])arrayConverter.Deserialize(typeof(byte[]), stream, serializer, null);
            
            var parameterType = serializer.GetContractParameterType(dest.Bytes, data);
            var (_, selector) = serializer.GetContractMeta(parameterType);
            using var ms = new MemoryStream(data, selector.Length, data.Length - selector.Length);
            var parameters = (IContractCallParameter) serializer.Deserialize(parameterType, ms);
            return new CallCall()
            {
                Dest = dest,
                Parameters = parameters,
                Data = data,
                Value = value,
                GasLimit = gasLimit
            };
        }

        public static CallCall Create(BigInteger value, BigInteger gasLimit, IContractCallParameter parameters, IBinarySerializer serializer)
        {
            var typedCall = new CallCall()
            {
                Value = value,
                GasLimit = gasLimit,
                Parameters = parameters,
            };
            var (dest, selector) = serializer.GetContractMeta(parameters.GetType());
            typedCall.Dest = new PublicKey() {Bytes = dest};
            using var ms = new MemoryStream();
            ms.Write(selector, 0, selector.Length);
            serializer.Serialize(parameters, ms);
            typedCall.Data = ms.ToArray();
            return typedCall;
        }
        
    }
}