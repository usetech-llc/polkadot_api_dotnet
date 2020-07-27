using System;
using System.IO;
using Polkadot.BinarySerializer.Extensions;

namespace Polkadot.BinarySerializer.Converters
{
    public abstract class MetadataIndexConverter : BaseInheritanceConverter
    {
        protected override void StoreTypeInfo(Stream stream, Type type, IBinarySerializer serializer, object[] parameters)
        {
            var (module, method) = GetIndex(type, serializer);
            stream.WriteByte(module);
            stream.WriteByte(method);
        }

        protected abstract (byte moduleIndex, byte itemIndex) GetIndex(Type type, IBinarySerializer serializer);

        protected override Type GetActualType(Stream stream, IBinarySerializer deserializer, object[] parameters)
        {
            var module = stream.ReadByteThrowIfStreamEnd();
            var item = stream.ReadByteThrowIfStreamEnd();

            return GetCallType(deserializer, module, item);
        }

        protected abstract Type GetCallType(IBinarySerializer deserializer, byte module, byte item);
    }
}