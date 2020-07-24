using System;
using System.IO;

namespace Polkadot.BinarySerializer.Converters
{
    public abstract class BaseInheritanceConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] parameters)
        {
            if (value == null)
            {
                return;
            }

            var type = value.GetType();
            StoreTypeInfo(stream, type, serializer, parameters);
            serializer.Serialize(value, stream);
        }

        protected abstract void StoreTypeInfo(Stream stream, Type type, IBinarySerializer serializer, object[] parameters);

        public object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] parameters)
        {
            var actualType = GetActualType(stream, deserializer, parameters);

            return deserializer.Deserialize(actualType, stream);
        }

        protected abstract Type GetActualType(Stream stream, IBinarySerializer deserializer, object[] parameters);
    }
}