using System;
using System.IO;
using OneOf;
using Polkadot.BinarySerializer;
using Polkadot.Utils;

namespace Polkadot.BinaryContracts
{
    public class EraDto: IBinarySerializable
    {
        public OneOf<ImmortalEra, MortalEra> Value { get; set; }

        public EraDto()
        {
        }

        public EraDto(OneOf<ImmortalEra, MortalEra> value)
        {
            Value = value;
        }

        public void Serialize(Stream stream, IBinarySerializer serializer)
        {
            Value.Switch(immortal => SerializeImmortal(stream, serializer, immortal), mortal => SerializeMortal(stream, serializer, mortal));
        }

        private void SerializeMortal(Stream stream, IBinarySerializer serializer, MortalEra mortal)
        {
            var quantizeFactor = Math.Max(1, mortal.Period >> 12);
            var low = (ushort)Math.Min(15, Math.Max(1, mortal.Period.TrailingZeroes() - 1));
            var high = (ushort)((mortal.Phase / quantizeFactor) << 4);
            var encoded = (ushort)(low | high);
            serializer.Serialize(encoded, stream);
        }

        private void SerializeImmortal(Stream stream, IBinarySerializer serializer, ImmortalEra immortal)
        {
            stream.WriteByte(0);
        }
    }
}