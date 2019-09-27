namespace Polkadot.Data
{
    public class RuntimeVersion
    {
        public ApiItem[] Api { get; set; }
        public uint AuthoringVersion { get; set; }
        public string ImplName { get; set; }
        public int ImplVersion { get; set; }
        public string SpecName { get; set; }
        public int SpecVersion { get; set; }
    }

    public struct ApiItem
    {
        public string Num { get; set; }
        public int Id { get; set; }
    };
}