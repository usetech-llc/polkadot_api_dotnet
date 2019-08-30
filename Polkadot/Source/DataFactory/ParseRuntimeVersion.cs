namespace Polkadot.DataFactory
{
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;
    using System.Collections.Generic;

    public class ParseRuntimeVersion : IParseFactory<RuntimeVersion>
    {
        public RuntimeVersion Parse(JObject json)
        {
            var apis = new List<ApiItem>();
            int i = 0;
            foreach (var item in json["result"]["apis"].Values())
            {
                apis.Add(new ApiItem { Id = ++i, Num = item.ToString() });
            }

            return new RuntimeVersion
            {
                AuthoringVersion = json["result"]["authoringVersion"].ToObject<uint>(),
                ImplName = json["result"]["implName"].ToString(),
                SpecVersion = json["result"]["specVersion"].ToObject<int>(),
                ImplVersion = json["result"]["implVersion"].ToObject<int>(),
                SpecName = json["result"]["specName"].ToString(),
                Api = apis.ToArray()
            };
        }
    }
}
