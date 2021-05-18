using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Polkadot.Api.Client.Serialization
{
    public class TextJsonSerializer : IJsonSerializer<TextJsonElement>
    {
        private readonly JsonSerializerOptions _options;
    
        public TextJsonSerializer(JsonSerializerOptions options, ISubstrateClient<TextJsonElement> client)
        {
            _options = new JsonSerializerOptions(options);
            if (!_options.Converters.OfType<BinaryJsonConverterSerializerHolder>().Any())
            {
                _options.Converters.Add(new BinaryJsonConverterSerializerHolder(() => client.BinarySerializer));
            }
        }
    
        public TextJsonElement DeserializeToElement(byte[] utf8Bytes)
        {
            var document = JsonDocument.Parse(utf8Bytes, new JsonDocumentOptions()
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            });

            return new TextJsonElement(document.RootElement, document, _options);
        }

        public byte[] Serialize<T>(T value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value, _options);
        }
    }
}