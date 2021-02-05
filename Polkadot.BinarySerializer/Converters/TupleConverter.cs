using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using OneOf;
using Polkadot.BinarySerializer.Extensions;

namespace Polkadot.BinarySerializer.Converters
{
    public class TupleConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] parameters)
        {
            var converters = parameters[0] as Type[];

            var tuple = (ITuple) value;
            for (int i = 0; i < tuple.Length; i++)
            {
                if (converters?[i] != null)
                {
                    serializer.GetConverter(converters[i]).Serialize(stream, tuple[i], serializer, (parameters[1] as object[][])?[i]);
                }
                else
                {
                    serializer.Serialize(tuple[i], stream);
                }
            }
        }

        public object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] parameters)
        {
            var converters = parameters[0] as Type[];

            var constructorParams = new object[type.GenericTypeArguments.Length];
            for (int i = 0; i < constructorParams.Length; i++)
            {
                if (converters?[i] != null)
                {
                    constructorParams[i] = deserializer.GetConverter(converters[i])
                        .Deserialize(type.GenericTypeArguments[i], stream, deserializer, (parameters[1] as object[][])?[i]);
                }
                else
                {
                    constructorParams[i] = deserializer.Deserialize(type.GenericTypeArguments[i], stream);
                }
            }

            var constructor = type.GetConstructor(type.GenericTypeArguments);
            return constructor.Invoke(constructorParams);
        }
    }
}