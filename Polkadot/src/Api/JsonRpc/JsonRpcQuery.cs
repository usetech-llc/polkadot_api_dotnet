namespace Polkadot.Api
{
    using Newtonsoft.Json.Linq;

    public struct JsonRpcQuery
    {
        public string Id { get; set; }
        public JObject Json { get; set; }
    };
}