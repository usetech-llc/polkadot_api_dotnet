using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Polkadot.Utils;

namespace Polkadot.Api.Client.Serialization
{
    public class BinaryJsonConverterAttribute : JsonConverterAttribute
    {
        public Type BinaryConverterType { get; set; }

        public BinaryJsonConverterAttribute() : base(null)
        {
        }

        protected BinaryJsonConverterAttribute(Type binaryConverterType) : base(null)
        {
            BinaryConverterType = binaryConverterType;
        }

        public override JsonConverter CreateConverter(Type typeToConvert)
        {
            var serializer = (JsonConverter)Activator.CreateInstance(
                typeof(BinaryJsonConverter<>)
                    .MakeGenericType(typeToConvert));
            ((IHasConverter) serializer).ConverterType = BinaryConverterType;
            return serializer;
        }
    }
}