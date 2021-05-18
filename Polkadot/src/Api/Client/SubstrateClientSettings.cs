using System;
using System.Text.Json;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.Api.Client.Serialization;
using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client
{
    public class SubstrateClientSettings
    {
        public static SubstrateClientSettings<TextJsonElement> Default()
        {
            return new(
                null, 
                _ => new BinarySerializer.BinarySerializer(), 
                client => new TextJsonSerializer(new JsonSerializerOptions(JsonSerializerDefaults.Web), client),
                TimeSpan.FromSeconds(30));
        }
    }
    
    public record SubstrateClientSettings<TJsonElement>(
        string RpcEndpoint, 
        Func<ISubstrateClient<TJsonElement>, IBinarySerializer> BinarySerializer, 
        Func<ISubstrateClient<TJsonElement>, IJsonSerializer<TJsonElement>> JsonSerializer,
        TimeSpan RpcTimeout
    ) where TJsonElement : IJsonElement<TJsonElement> 
    {
    }
}