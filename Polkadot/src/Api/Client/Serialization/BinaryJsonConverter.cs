using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Extensions;
using Polkadot.Utils;

namespace Polkadot.Api.Client.Serialization
{
    public class BinaryJsonConverter<T> : JsonConverter<T>, IHasConverter
    {
        public override bool HandleNull => false;
        public IBinarySerializer BinarySerializer { get; set; } = null;
        public Type ConverterType { get; set; }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            var str = reader.GetString();
            if (typeToConvert == typeof(string))
            {
                return (T)(object)str;
            }
            
            var serializer = GetBinarySerializer(options);
            if (string.IsNullOrEmpty(str))
            {
                return (T)serializer.Deserialize(typeToConvert, Array.Empty<byte>());
            }

            if (!str.StartsWith("0x"))
            {
                throw new FormatException("Binary serialized data expected to be string starting with 0x");
            }

            var bytes = str.HexToByteArray();
            if (ConverterType != null)
            {
                using var ms = new MemoryStream(bytes);
                var deserialized = serializer.GetConverter(ConverterType).Deserialize(typeToConvert, ms, serializer, Array.Empty<object>());
                if (ms.TryReadByte(out var _))
                {
                    throw new NotAllDataUsedException();
                }

                return (T)deserialized;
            }
            return serializer.DeserializeAssertReadAll<T>(bytes);
        }

        private IBinarySerializer GetBinarySerializer(JsonSerializerOptions options)
        {
            BinarySerializer ??= options.Converters.OfType<BinaryJsonConverterSerializerHolder>().First().BinarySerializer();
            return BinarySerializer;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var serializer = GetBinarySerializer(options);
            if (ConverterType != null)
            {
                using var ms = new MemoryStream();
                serializer.GetConverter(ConverterType).Serialize(ms, value, serializer, Array.Empty<object>());
                return;
            }

            var str = serializer.Serialize(value).ToPrefixedHexString();
            writer.WriteStringValue(str);
        }
    }
}