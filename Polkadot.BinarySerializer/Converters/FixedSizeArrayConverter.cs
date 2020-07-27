using System;
using System.IO;
using Polkadot.BinarySerializer;

namespace Polkadot.BinarySerializer.Converters
{
    public class FixedSizeArrayConverter : BaseArrayConverter
    {
        protected override int GetSize(Type type, Stream stream, IBinarySerializer deserializer, object[] param)
        {
            return (int) param[2];
        }
    }
}