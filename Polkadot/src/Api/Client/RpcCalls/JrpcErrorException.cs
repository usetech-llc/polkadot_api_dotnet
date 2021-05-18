using System;
using Polkadot.Api.Client.Serialization;

namespace Polkadot.Api.Client.RpcCalls
{
    public class JrpcErrorException<TJsonElement> : Exception  where TJsonElement : IJsonElement<TJsonElement>
    {
        public const long MethodNotFound = -32601;
        
        public long? Code { get; }
        public TJsonElement Error { get; }

        public JrpcErrorException(long? code, TJsonElement error)
        {
            Code = code;
            Error = error;
        }

        public override string Message => $"Rpc call failed with error:\n{Error.ToJson()}";
    }
}