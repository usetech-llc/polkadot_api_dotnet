namespace Polkadot.BinarySerializer.Types
{
    /// <summary>
    /// "no value" type. Like () in the rust.
    /// </summary>
    public class Unit
    {
        public static Unit Instance { get; } = new Unit();
    }
}