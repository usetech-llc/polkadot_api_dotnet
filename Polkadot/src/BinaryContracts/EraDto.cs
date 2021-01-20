using System;
using System.IO;
using OneOf;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Extensions;
using Polkadot.Data;
using Polkadot.Utils;

namespace Polkadot.BinaryContracts
{
    public class EraDto: IBinarySerializable, IBinaryDeserializable
    {
        public OneOf<ImmortalEra, MortalEra> Value { get; set; }

        public EraDto()
        {
        }

        public EraDto(OneOf<ImmortalEra, MortalEra> value)
        {
            Value = value;
        }

        public ulong Birth(ulong current)
        {
            return Value.Match(
                _ => 0UL,
                mortal => (Math.Max(current, mortal.Phase) - mortal.Phase) / mortal.Period * mortal.Period + mortal.Phase
            );
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

        public object Deserialize(Stream stream, IBinarySerializer serializer)
        {
            var b0 = stream.ReadByteThrowIfStreamEnd();
            if (b0 == 0)
            {
                return new EraDto(new ImmortalEra());
            }

            var b1 = stream.ReadByteThrowIfStreamEnd();
            return ParseMortalEra(b0, b1);
        }

        private EraDto ParseMortalEra(byte b0, byte b1)
        {
            var ul0 = (ulong) b0;
            var ul1 = (ulong) b1;
            var encoded = ul0  + (ul1 << 8);
            var period = 2UL << (int)(encoded % (1 << 4));
            var quantizeFactor = Math.Max(1, period >> 12);
            var phase = (encoded >> 4) * quantizeFactor;
            if (period < 4 || phase >= period)
            {
                throw new ArgumentException($"{new[] {b0, b1}.ToHexString()} is not a valid representation of Era.");
            }

            return new EraDto(new MortalEra(period, phase));
        }
    }
}