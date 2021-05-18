using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Polkadot.BinarySerializer;

namespace Polkadot.Api.Client.Serialization
{
    /// <summary>
    /// Not a converter, but a way to pass current BinarySerializer to real BinaryJsonConverters.
    /// </summary>
    internal class BinaryJsonConverterSerializerHolder : JsonConverter<BinarySerializerHolder>
    {
        public Func<IBinarySerializer> BinarySerializer { get; set; }
        

        public BinaryJsonConverterSerializerHolder(Func<IBinarySerializer> binarySerializer)
        {
            BinarySerializer = binarySerializer;
        }
        
        public override BinarySerializerHolder Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, BinarySerializerHolder value, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }
    }
}