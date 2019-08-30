namespace Polkadot.Data
{
    public class SignedBlock
    {
        public Block block { get; set; }
        public string justification { get; set; }
    }

    public struct Block
    {
        public BlockHeader header { get; set; }
        public string[] extrinsic { get; set; }
    };
}