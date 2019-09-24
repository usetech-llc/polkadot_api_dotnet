namespace Polkadot.Data
{
    public class SignedBlock
    {
        public Block Block { get; set; }
        public string Justification { get; set; }
    }

    public struct Block
    {
        public BlockHeader Header { get; set; }
        public string[] Extrinsic { get; set; }
    };
}