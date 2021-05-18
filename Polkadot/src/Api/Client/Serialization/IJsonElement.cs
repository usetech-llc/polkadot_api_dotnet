using System;
using System.Threading.Tasks;

namespace Polkadot.Api.Client.Serialization
{
    public interface IJsonElement<TJsonElement> : IDisposable where TJsonElement : IJsonElement<TJsonElement>
    {
        bool TryGetString(out string value);
        bool TryGetLong(out long value);
        bool TryGetProperty(string propertyName, out TJsonElement value);
        ValueTask<T> DeserializeObject<T>();
        TJsonElement Clone();
        string ToJson();
    }
}