using Newtonsoft.Json;
using Polkadot.JsonConverters;

namespace Polkadot.Data
{
    public class RuntimeVersion
    {
        [JsonProperty(ItemConverterType = typeof(TupleConverter))]
        public (string, int)[] Apis { get; set; }
        public uint AuthoringVersion { get; set; }
        public string ImplName { get; set; }
        public int ImplVersion { get; set; }
        public string SpecName { get; set; }
        public int SpecVersion { get; set; }
        public int TransactionVersion { get; set; }
    }
}