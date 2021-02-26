using System.IO;
using System.Numerics;
using Polkadot.BinarySerializer;
using PublicKey = Polkadot.DataStructs.PublicKey;

namespace Polkadot.BinaryContracts.Calls.Contracts
{
    public class TypedContractCall : IBinarySerializable, IBinaryDeserializable
    {
        private CallCall _call = new CallCall();

        public PublicKey Dest
        {
            get => _call.Dest;
            set => _call.Dest = value;
        }

        public BigInteger Value
        {
            get => _call.Value;
            set => _call.Value = value;
        }

        public BigInteger GasLimit 
        { 
            get => _call.GasLimit;
            set => _call.GasLimit = value; 
        }

        public IContractCallParameter Parameters { get; set; }
        
        public void Serialize(Stream stream, IBinarySerializer serializer)
        {
            serializer.Serialize(_call, stream);
        }

        public object Deserialize(Stream stream, IBinarySerializer serializer)
        {
            _call = serializer.Deserialize<CallCall>(stream);
            var parameterType = serializer.GetContractParameterType(_call.Dest.Bytes, _call.Data);
            var (_, selector) = serializer.GetContractMeta(parameterType);
            using var ms = new MemoryStream(_call.Data, selector.Length, _call.Data.Length - selector.Length);
            Parameters = (IContractCallParameter)serializer.Deserialize(parameterType, ms);
            return this;
        }

        public TypedContractCall()
        {
        }

        public static TypedContractCall Create(BigInteger value, BigInteger gasLimit, IContractCallParameter parameters, IBinarySerializer serializer)
        {
            var typedCall = new TypedContractCall()
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
            typedCall._call.Data = ms.ToArray();
            return typedCall;
        }
        
    }
}