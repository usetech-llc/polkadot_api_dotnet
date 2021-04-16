using System;
using System.Text.Json;

namespace Polkadot.Api.Client.RpcCalls
{
    public class JrpcErrorException : Exception
    {
        public long? Code { get; }
        public JsonElement Error { get; }

        public JrpcErrorException(long? code, JsonElement error)
        {
            Code = code;
            Error = error;
        }

        public override string Message => $"Rpc call failed with error:\n{Error.ToString()}";
    }
}