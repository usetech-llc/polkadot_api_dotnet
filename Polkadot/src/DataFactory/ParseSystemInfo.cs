namespace Polkadot.DataFactory
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;

    /// <summary>
    /// Call 4 methods and put them together in a single object
    /// system_chain
    /// system_name
    /// system_version
    /// system_properties
    /// </summary>
    public class ParseSystemInfo : IParseFactory<SystemInfo>
    {
        public SystemInfo Parse(JObject jsonObject)
        {
            var tokenDecimals = jsonObject["item3"].HasValues ? jsonObject["item3"].Value<string>("tokenDecimals") : "";
            var tokenSymbol = jsonObject["item3"].HasValues ? jsonObject["item3"].Value<string>("tokenSymbol") : "";

            return new SystemInfo
            {
                ChainName = jsonObject["item0"].ToString(),
                ChainId = jsonObject["item1"].ToString(),
                Version = jsonObject["item2"].ToString(),
                TokenDecimals = string.IsNullOrEmpty(tokenDecimals) ? 0 : int.Parse(tokenDecimals), 
                TokenSymbol = tokenSymbol
            };
        }
    }
}
