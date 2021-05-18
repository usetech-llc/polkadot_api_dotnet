using Polkadot.Api.Client.Serialization;

namespace Polkadot.Extensions
{
    public static class JsonElementExtensions
    {
        public static string GetStringOrNull<T>(this T element) where T : IJsonElement<T>
        {
            return element.TryGetString(out var value) ? value : null;
        }
    }
}