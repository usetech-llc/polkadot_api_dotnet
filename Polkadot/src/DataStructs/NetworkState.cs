namespace Polkadot.Data
{
    public class NetworkState
    {
        public uint AverageDownloadPerSec { get; set; }
        public uint AverageUploadPerSec { get; set; }
        public ConnectedPeer[] ConnectedPeers { get; set; }
        public string[] ExternalAddresses { get; set; }
        public string[] ListenedAddresses { get; set; }
        public NotConnectedPeer[] NotConnectedPeers { get; set; }
        public string PeerId { get; set; }
        public string Peerset { get; set; }
    }

    public class NotConnectedPeer
    {
        public string Key { get; set; }
        public NotConnectedPeerInfo NotConnectedPeerInfo { get; set; }
    }

    public class NotConnectedPeerInfo
    {
        public string[] KnownAddresses { get; set; }
    }

    public class ConnectedPeer
    {
        public string Key { get; set; }
        public ConnectedPeerInfo ConnectedPeerInfo { get; set; }
    }

    public class ConnectedPeerInfo
    {
        public bool Enabled { get; set; }
        public Endpoint Endpoint { get; set; }
        public string[] KnownAddresses { get; set; }
        public ConnectedPeerTime LatestPingTime { get; set; }
        public bool Open { get; set; }
        public string VersionString { get; set; }
    }

    public class ConnectedPeerTime
    {
        public ulong Nanos { get; set; }
        public ulong Secs { get; set; }
    }

    public class Endpoint
    {
        public string Dialing { get; set; }
    }

    public class PeersInfo
    {
        public int Count { get; set; }
        public PeerInfo[] Peers { get; set; }
    };

    public class PeerInfo
    {
        public string BestHash { get; set; }
        public ulong BestNumber { get; set; }
        public string PeerId { get; set; }
        public uint ProtocolVersion { get; set; }
        public string Roles { get; set; }
    }

    public class SystemHealth
    {
        public long Peers { get; set; }
        public bool IsSyncing { get; set; }
        public bool ShouldHavePeers { get; set; }
    }
}