using System;
using System.Text.Json;

namespace Polkadot.Api.Client.RpcCalls
{
    public class JrpcDeserializationException : Exception
    {
        public JsonElement Response { get; set; }

        public JrpcDeserializationException(JsonElement response)
        {
            Response = response;
        }

        public JrpcDeserializationException(JsonElement response, Exception innerException) : base("", innerException)
        {
            Response = response;
        }

        public override string Message => $"Failed to deserialize rpc response:\n{Response.ToString()}";
    }
}