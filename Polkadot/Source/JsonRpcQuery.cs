using Newtonsoft.Json.Linq;

namespace Polkadot.Source
{
    public struct JsonRpcQuery
    {
        public int Id { get; set; }
        public JObject Json { get; set; }
    };
}