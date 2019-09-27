namespace Polkadot.DataFactory
{
    using Newtonsoft.Json.Linq;

    public interface IParseFactory<T>
    {
        T Parse(JObject json);
    }
}