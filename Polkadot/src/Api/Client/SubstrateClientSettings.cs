using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client
{
    public record SubstrateClientSettings(string RpcEndpoint, IBinarySerializer BinarySerializer)
    {
        public static SubstrateClientSettings Default()
        {
            return new(null, new BinarySerializer.BinarySerializer());
        }
    }
}