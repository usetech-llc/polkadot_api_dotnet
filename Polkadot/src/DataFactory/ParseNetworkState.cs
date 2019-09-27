namespace Polkadot.DataFactory
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;
    using System.Collections.Generic;

    public class ParseNetworkState : IParseFactory<NetworkState>
    {
        public NetworkState Parse(JObject json)
        {
            dynamic djson = JsonConvert.DeserializeObject(json["result"].ToString());

            var peerList = new List<ConnectedPeer>();
            foreach (var cpeer in djson["connectedPeers"])
            {
                var cp = new ConnectedPeer
                {
                    Key = cpeer.Name.ToString()
                };
                cp.ConnectedPeerInfo = new ConnectedPeerInfo();

                cp.ConnectedPeerInfo.Enabled = cpeer.Value["enabled"].ToObject<bool>();

                // endpoint -> dealing
                cp.ConnectedPeerInfo.Endpoint = new Endpoint
                {
                    Dialing = cpeer.Value["dialing"]
                };

                // knownAddresses
                var addrs = new List<string>();
                foreach (var addr in cpeer.Value["knownAddresses"])
                {
                    addrs.Add(addr.ToString());
                }
                cp.ConnectedPeerInfo.KnownAddresses = addrs.ToArray();

                // latestPingTime
                var cpt = new ConnectedPeerTime();
                cpt.Nanos = cpeer.Value["latestPingTime"]["nanos"].ToObject<ulong>();
                cpt.Secs = cpeer.Value["latestPingTime"]["secs"].ToObject<ulong>();
                cp.ConnectedPeerInfo.LatestPingTime = cpt;

                cp.ConnectedPeerInfo.Open = cpeer.Value["open"].ToObject<bool>();
                cp.ConnectedPeerInfo.VersionString = cpeer.Value["versionString"].ToString();

                peerList.Add(cp);
            }

            var addrsList = new List<string>();
            foreach (var addr in djson["externalAddresses"])
            {
                addrsList.Add(addr.ToString());
            }

            var addrsLisList = new List<string>();
            foreach (var addr in djson["listenedAddresses"])
            {
                addrsLisList.Add(addr.ToString());
            }

            var ncpList = new List<NotConnectedPeer>();
            foreach (var ncp in djson["notConnectedPeers"])
            {
                var addrList = new List<string>();
                foreach (var addr2 in ncp.Value["knownAddresses"])
                {
                    addrList.Add(addr2.ToString());
                }

                var item = new NotConnectedPeer
                {
                    Key = ncp.Name.ToString(),
                    NotConnectedPeerInfo = new NotConnectedPeerInfo { KnownAddresses = addrList.ToArray() }
                };
            }

            return new NetworkState
            {
                AverageDownloadPerSec = djson["averageDownloadPerSec"].ToObject<uint>(),
                AverageUploadPerSec = djson["averageUploadPerSec"].ToObject<uint>(),
                PeerId = djson["peerId"].ToString(),
                Peerset = djson["peerset"].ToString(),
                ConnectedPeers = peerList.ToArray()
            };
        }
    }
}
