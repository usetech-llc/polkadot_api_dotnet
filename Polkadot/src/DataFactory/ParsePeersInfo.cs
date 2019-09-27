namespace Polkadot.DataFactory
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;
    using System.Collections.Generic;

    public class ParsePeersInfo : IParseFactory<PeersInfo>
    {
        public PeersInfo Parse(JObject json)
        {
            dynamic djson = JsonConvert.DeserializeObject(json["result"].ToString());
            int count = djson.Count;

            var peerList = new List<PeerInfo>();
            foreach(var peer in djson)
            {
                peerList.Add(new PeerInfo
                {
                    BestHash = peer["bestHash"].ToString(),
                    BestNumber = peer["bestNumber"].ToObject<ulong>(),
                    PeerId = peer["peerId"].ToString(),
                    ProtocolVersion = peer["protocolVersion"].ToObject<uint>(),
                    Roles = peer["roles"].ToString(),
                });
            }

            return new PeersInfo
            {
                Count = count,
                Peers = peerList.ToArray()
            };
        }
    }
}
