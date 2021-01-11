using System;
using System.IO;
using System.Linq;
using System.Reflection;
using OneOf;
using Polkadot.BinarySerializer.Extensions;

namespace Polkadot.BinarySerializer.Converters
{
    public class OneOfConverter : IBinaryConverter
    {
        private static string[] _constructors = {
            "FromT0",
            "FromT1",
            "FromT2",
            "FromT3",
            "FromT4",
            "FromT5",
            "FromT6",
            "FromT7",
            "FromT8",
            "FromT9",
            "FromT10",
            "FromT11",
            "FromT12",
            "FromT13",
            "FromT14",
            "FromT15",
            "FromT16",
            "FromT17",
            "FromT18",
            "FromT19",
            "FromT20",
            "FromT21",
            "FromT22",
            "FromT23",
            "FromT24",
            "FromT25",
            "FromT26",
            "FromT27",
            "FromT28",
            "FromT29",
            "FromT30",
            "FromT31",
            "FromT32",
        };
        
        public void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] parameters)
        {
            var converters = parameters[0] as Type[];

            var oneOf = (IOneOf) value;
            var innerValue = oneOf.Value;
            var index = oneOf.Index;
            stream.WriteByte((byte)index);
            if (converters?[index] != null)
            {
                var converter = serializer.GetConverter(converters[index]);
                converter.Serialize(stream, innerValue, serializer, (parameters[1] as object[][])?[index]);
            }
            else
            {
                serializer.Serialize(innerValue, stream);
            }
        }

        public object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] parameters)
        {
            var converters = parameters[0] as Type[];
            var index = stream.ReadByteThrowIfStreamEnd();
            var innerType = type.GetGenericArguments()[index];
            object innerValue;
            if (converters?[index] != null)
            {
                var converter = deserializer.GetConverter(converters[index]);
                innerValue = converter.Deserialize(innerType, stream, deserializer, (parameters[1] as object[][])?[index]);
            }
            else
            {
                innerValue = deserializer.Deserialize(innerType, stream);
            }
            var constructMethod = type.GetMethod(_constructors[index], new[] {innerType});
            return constructMethod!.Invoke(null, new[] {innerValue});
        }
    }
}