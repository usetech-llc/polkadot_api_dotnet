namespace Polkadot.DataFactory
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;

    public class ParseSystemHealth : IParseFactory<SystemHealth>
    {
        public SystemHealth Parse(JObject json)
        {
            dynamic djson = JsonConvert.DeserializeObject(json["result"].ToString());

            return new SystemHealth
            {
                Peers = djson["peers"].ToObject<int>(),
                IsSyncing = djson["isSyncing"].ToObject<bool>(),
                ShouldHavePeers = djson["shouldHavePeers"].ToObject<bool>()
            };
        }
    }
}
