using System.Text.Json;
using System.Threading.Tasks;

namespace Polkadot.Api.Client.Serialization
{
    public interface IJsonSerializer<TElement>
    {
        TElement DeserializeToElement(byte[] utf8Bytes);
        byte[] Serialize<T>(T value);
    }
}