namespace Polkadot.Api.Client.Modules.State.Model.Interfaces
{
    public interface IConstantMeta
    {
        string GetName();
        string GetValue();

        byte[] GetValueBytes();
    }
}