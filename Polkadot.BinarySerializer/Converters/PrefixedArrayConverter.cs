using System;
using System.IO;
using Polkadot.BinarySerializer;

namespace Polkadot.BinarySerializer.Converters
{
    public class PrefixedArrayConverter : BaseArrayConverter
    {
        protected override int GetSize(Type type, Stream stream, IBinarySerializer deserializer, object[] param)
        {
            return (int) Scale.DecodeCompactInteger(stream).Value;
        }
    }
}