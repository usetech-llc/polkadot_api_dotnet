namespace Polkadot.DataFactory
{
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;

    public class ParseBlockHash : IParseFactory<BlockHash>
    {
        public BlockHash Parse(JObject json)
        {
            return new BlockHash
            {
                Hash = json["result"].ToString()
            };
        }
    }
}
