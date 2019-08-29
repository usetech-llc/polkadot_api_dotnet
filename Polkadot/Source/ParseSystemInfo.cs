using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Polkadot.Source
{
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
            return new SystemInfo
            {
                chainName = jsonObject["item0"].ToString(),
                chainId = jsonObject["item1"].ToString(),
                version = jsonObject["item2"].ToString(),
                tokenDecimals = jsonObject["item3"]["tokenDecimals"].ToObject<int>(),
                tokenSymbol = jsonObject["item3"]["tokenSymbol"].ToString()
            };
        }
    }
}
