﻿namespace Polkadot.DataFactory
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
            dynamic djson = JsonConvert.DeserializeObject(jsonObject["item3"]?.ToString() ?? string.Empty);
            var tokenDecimals = djson["tokenDecimals"]?.ToObject<int>() ?? 0;
            var tokenSymbol = djson["tokenSymbol"]?.ToString() ?? string.Empty;

            return new SystemInfo
            {
                ChainName = jsonObject["item0"].ToString(),
                ChainId = jsonObject["item1"].ToString(),
                Version = jsonObject["item2"].ToString(),
                TokenDecimals = tokenDecimals,
                TokenSymbol = tokenSymbol
            };
        }
    }
}
