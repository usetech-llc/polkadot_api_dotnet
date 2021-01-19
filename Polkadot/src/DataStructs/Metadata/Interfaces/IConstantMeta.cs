namespace Polkadot.DataStructs.Metadata.Interfaces
{
    public interface IConstantMeta
    {
        string GetName();
        string GetValue();

        byte[] GetValueBytes();
    }
}