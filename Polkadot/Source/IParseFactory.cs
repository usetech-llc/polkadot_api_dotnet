using Newtonsoft.Json.Linq;

namespace Polkadot.Source
{
    public interface IParseFactory<T>
    {
        T Parse(JObject json);
    }
}