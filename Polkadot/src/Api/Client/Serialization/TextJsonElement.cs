using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Polkadot.Api.Client.Serialization
{
    public readonly struct TextJsonElement : IJsonElement<TextJsonElement>
    {
        private readonly JsonElement _element;
        private readonly JsonDocument _document;
        private readonly JsonSerializerOptions _options;

        public TextJsonElement(JsonElement element, JsonDocument document, JsonSerializerOptions options)
        {
            _element = element;
            _document = document;
            _options = options;
        }
        
        public void Dispose()
        {
            _document?.Dispose();
        }

        public bool TryGetString(out string value)
        {
            if (_element.ValueKind == JsonValueKind.String)
            {
                value = _element.GetString();
                return true;
            }

            value = null;
            return false;
        }

        public bool TryGetLong(out long value)
        {
            if (_element.ValueKind == JsonValueKind.Number)
            {
                value = _element.GetInt64();
                return true;
            }

            value = 0;
            return false;
        }

        public bool TryGetProperty(string propertyName, out TextJsonElement value)
        {
            var hasProperty = _element.TryGetProperty(propertyName, out var jsonElement);
            if (hasProperty)
            {
                value = new TextJsonElement(jsonElement, _document, _options);
                return true;
            }

            value = default;
            return false;
        }

        public async ValueTask<T> DeserializeObject<T>()
        {
            using var ms = new MemoryStream();
            await using var writer = new Utf8JsonWriter(ms);
            _element.WriteTo(writer);
            await writer.FlushAsync();
            var deserialized = JsonSerializer.Deserialize<T>(ms.ToArray(), _options);
            
            return deserialized;

        }

        public TextJsonElement Clone()
        {
            return new(_element.Clone(), null, _options);
        }

        public string ToJson()
        {
            return _element.ToString();
        }
    }
}