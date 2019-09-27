namespace Polkadot.DataFactory
{
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;

    public class ParseFinalizedHead : IParseFactory<FinalHead>
    {
        public FinalHead Parse(JObject json)
        {
            return new FinalHead
            {
                BlockHash = json["result"].ToString()
            };
        }
    }
}
