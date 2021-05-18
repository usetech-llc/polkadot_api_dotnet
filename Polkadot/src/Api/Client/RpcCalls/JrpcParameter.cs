using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Polkadot.Api.Client.RpcCalls
{
    public record JrpcParameter(
        [property: JsonPropertyName("id")] long Id,
        [property: JsonPropertyName("method")] string Method,
        [property: JsonPropertyName("params")] IEnumerable<string> @params,
        [property: JsonPropertyName("jsonrpc")] string Jsonrpc = "2.0"
    );
}