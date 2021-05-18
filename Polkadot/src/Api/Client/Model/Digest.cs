namespace Polkadot.Api.Client.Model
{
    public class Digest<THash>
    {
        public DigestItem<THash>[] Logs { get; set; }
    }
}