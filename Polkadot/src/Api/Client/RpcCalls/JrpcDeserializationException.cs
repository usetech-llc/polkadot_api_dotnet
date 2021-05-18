using System;
using Polkadot.Api.Client.Serialization;

namespace Polkadot.Api.Client.RpcCalls
{
    public class JrpcDeserializationException<TJsonElement> : Exception where TJsonElement : IJsonElement<TJsonElement>
    {
        private readonly Type _deserializeInto;
        public TJsonElement Response { get; set; }

        public JrpcDeserializationException(TJsonElement response, Type deserializeInto)
        {
            _deserializeInto = deserializeInto;
            Response = response;
        }

        public JrpcDeserializationException(TJsonElement response, Type deserializeInto, Exception innerException) : base("", innerException)
        {
            _deserializeInto = deserializeInto;
            Response = response;
        }

        public override string Message => $"Failed to deserialize rpc response into {_deserializeInto.FullName}:\n{Response.ToString()}";
    }
}