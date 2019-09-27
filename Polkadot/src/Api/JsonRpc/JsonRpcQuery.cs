namespace Polkadot.Api
{
    using Newtonsoft.Json.Linq;

    public struct JsonRpcQuery
    {
        public int Id { get; set; }
        public JObject Json { get; set; }
    };
}