using System;
using System.IO;
using System.Runtime.CompilerServices;
using Polkadot.BinarySerializer.Extensions;
using Polkadot.BinarySerializer.Types;

namespace Polkadot.BinarySerializer.Converters
{
    /// <summary>
    /// Big endian. Cuts leading zeroes, so 64bit 0x01 will be serialized into 1 and deserializing 0x01 into 64bit will assume there are cut leading zeroes and it wont throw exception.
    /// </summary>
    public class BigEndianUncheckedConverter : IBinaryConverter
    {
        public void Serialize(Stream stream, object value, IBinarySerializer serializer, object[] parameters)
        {
            switch (value.GetType())
            {
                case {} t when t == typeof(byte): WriteInteger((byte) value, 1, stream); break;
                case {} t when t == typeof(sbyte): WriteInteger((ulong)(sbyte) value, 1, stream); break;
                case {} t when t == typeof(short): WriteInteger((ulong)(short) value, 2, stream); break;
                case {} t when t == typeof(ushort): WriteInteger((ushort) value, 2, stream); break;
                case {} t when t == typeof(int): WriteInteger((ulong)(int) value, 4, stream); break;
                case {} t when t == typeof(uint): WriteInteger((uint) value, 4, stream); break;
                case {} t when t == typeof(long): WriteInteger((ulong)(long) value, 8, stream); break;
                case {} t when t == typeof(ulong): WriteInteger((ulong) value, 8, stream); break;
                case {} t when t == typeof(Int128): WriteInt128((Int128)value, stream); break;
                case {} t when t == typeof(UInt128): WriteUInt128((UInt128)value, stream); break;
                case {} t when t == typeof(Int256): WriteInt256((Int256)value, stream); break;
                case {} t when t == typeof(UInt256): WriteUInt256((UInt256)value, stream); break;
                case {} t when t == typeof(Int512): WriteInt512((Int512)value, stream); break;
                case {} t when t == typeof(UInt512): WriteUInt512((UInt512)value, stream); break;
                
                default: throw new ArgumentException("Unsupported integer type.", nameof(value));
            }
        }

        private void WriteUInt512(UInt512 value, Stream stream)
        {
            var (ul0, ul1, ul2, ul3, ul4, ul5, ul6, ul7) = value;
            WriteInteger(ul7, 8, stream);
            WriteInteger(ul6, 8, stream);
            WriteInteger(ul5, 8, stream);
            WriteInteger(ul4, 8, stream);
            WriteInteger(ul3, 8, stream);
            WriteInteger(ul2, 8, stream);
            WriteInteger(ul1, 8, stream);
            WriteInteger(ul0, 8, stream);
        }

        private void WriteInt512(Int512 value, Stream stream)
        {
            var (ul0, ul1, ul2, ul3, ul4, ul5, ul6, ul7) = value;
            WriteInteger(ul7, 8, stream);
            WriteInteger(ul6, 8, stream);
            WriteInteger(ul5, 8, stream);
            WriteInteger(ul4, 8, stream);
            WriteInteger(ul3, 8, stream);
            WriteInteger(ul2, 8, stream);
            WriteInteger(ul1, 8, stream);
            WriteInteger(ul0, 8, stream);
        }

        private void WriteUInt256(UInt256 value, Stream stream)
        {
            var (ul0, ul1, ul2, ul3) = value;
            WriteInteger(ul3, 8, stream);
            WriteInteger(ul2, 8, stream);
            WriteInteger(ul1, 8, stream);
            WriteInteger(ul0, 8, stream);
        }

        private void WriteInt256(Int256 value, Stream stream)
        {
            var (ul0, ul1, ul2, ul3) = value;
            WriteInteger(ul3, 8, stream);
            WriteInteger(ul2, 8, stream);
            WriteInteger(ul1, 8, stream);
            WriteInteger(ul0, 8, stream);
        }

        private void WriteUInt128(UInt128 value, Stream stream)
        {
            var (ul0, ul1) = value;
            WriteInteger(ul1, 8, stream);
            WriteInteger(ul0, 8, stream);
        }

        private void WriteInt128(Int128 value, Stream stream)
        {
            var (ul0, ul1) = value;
            WriteInteger(ul1, 8, stream);
            WriteInteger(ul0, 8, stream);
        }

        private void WriteInteger(ulong value, int size, Stream stream)
        {
            Span<byte> bytes = stackalloc byte[size];
            for (int i = 0; i < size; i++)
            {
                bytes[i] = (byte) ((value >> 8 * i) & 0xff);
            }

            for (int i = size - 1; i >= 0; i--)
            {
                stream.WriteByte(bytes[i]);
            }
        }

        private ulong ReadInteger(int size, Stream stream)
        {
            Span<byte> bytes = stackalloc byte[size];
            var bytesRead = ReadBytes(stream, ref bytes);

            var integer = ReadUlong(bytes, bytesRead);

            return integer;
        }

        private static ulong ReadUlong(Span<byte> bytes, int size)
        {
            ulong integer = 0;
            for (int j = 0; j < size; j++)
            {
                integer <<= 8;
                integer |= bytes[j];
            }

            return integer;
        }

        private static int ReadBytes(Stream stream, ref Span<byte> bytes)
        {
            int bytesRead;
            for (bytesRead = 0; bytesRead < bytes.Length; bytesRead++)
            {
                if (stream.TryReadByte(out var b))
                {
                    bytes[bytesRead] = b;
                }
                else
                {
                    break;
                }
            }

            return bytesRead;
        }

        public object Deserialize(Type type, Stream stream, IBinarySerializer deserializer, object[] parameters)
        {
            unchecked
            {
                // ReSharper disable once HeapView.BoxingAllocation
                return type switch
                {
                    {} when type == typeof(byte) => stream.ReadByteThrowIfStreamEnd(),
                    {} when type == typeof(sbyte) => (sbyte)stream.ReadByteThrowIfStreamEnd(),
                    {} when type == typeof(short) => (short)ReadInteger(2, stream),
                    {} when type == typeof(ushort) => (ushort)ReadInteger(2, stream),
                    {} when type == typeof(int) => (int)ReadInteger(4, stream),
                    {} when type == typeof(uint) => (uint)ReadInteger(4, stream),
                    {} when type == typeof(long) => (long)ReadInteger(8, stream),
                    {} when type == typeof(ulong) => ReadInteger(8, stream),
                    {} when type == typeof(Int128) => ReadInt128(stream),
                    {} when type == typeof(UInt128) => ReadUInt128(stream),
                    {} when type == typeof(Int256) => ReadInt256(stream),
                    {} when type == typeof(UInt256) => ReadUInt256(stream),
                    {} when type == typeof(Int512) => ReadInt512(stream),
                    {} when type == typeof(UInt512) => ReadUInt512(stream),
                    
                    _ => throw new ArgumentException("Unsupported integer type", nameof(type))
                };
            }
        }

        private Int128 ReadInt128(Stream stream)
        {
            Span<byte> bytes = stackalloc byte[16];
            var bytesRead = ReadBytes(stream, ref bytes);
            AdjustBe(bytesRead, bytes, 16);
            var ul0 = ReadUlong(bytes[8..16], 8);
            var ul1 = ReadUlong(bytes[..8], 8);
            return new Int128(ul0, ul1);
        }

        private UInt128 ReadUInt128(Stream stream)
        {
            Span<byte> bytes = stackalloc byte[16];
            var bytesRead = ReadBytes(stream, ref bytes);
            AdjustBe(bytesRead, bytes, 16);
            var ul0 = ReadUlong(bytes[8..16], 8);
            var ul1 = ReadUlong(bytes[..8], 8);
            return new UInt128(ul0, ul1);
        }

        private Int256 ReadInt256(Stream stream)
        {
            Span<byte> bytes = stackalloc byte[32];
            var bytesRead = ReadBytes(stream, ref bytes);
            AdjustBe(bytesRead, bytes, 32);
            var ul0 = ReadUlong(bytes[24..32], 8);
            var ul1 = ReadUlong(bytes[16..24], 8);
            var ul2 = ReadUlong(bytes[8..16], 8);
            var ul3 = ReadUlong(bytes[..8], 8);
            return new Int256(ul0, ul1, ul2, ul3);
        }

        private UInt256 ReadUInt256(Stream stream)
        {
            Span<byte> bytes = stackalloc byte[32];
            var bytesRead = ReadBytes(stream, ref bytes);
            AdjustBe(bytesRead, bytes, 32);
            var ul0 = ReadUlong(bytes[24..32], 8);
            var ul1 = ReadUlong(bytes[16..24], 8);
            var ul2 = ReadUlong(bytes[8..16], 8);
            var ul3 = ReadUlong(bytes[..8], 8);
            return new UInt256(ul0, ul1, ul2, ul3);
        }

        private Int512 ReadInt512(Stream stream)
        {
            Span<byte> bytes = stackalloc byte[64];
            var bytesRead = ReadBytes(stream, ref bytes);
            AdjustBe(bytesRead, bytes, 32);
            var ul0 = ReadUlong(bytes[56..64], 8);
            var ul1 = ReadUlong(bytes[48..56], 8);
            var ul2 = ReadUlong(bytes[40..48], 8);
            var ul3 = ReadUlong(bytes[32..40], 8);
            var ul4 = ReadUlong(bytes[24..32], 8);
            var ul5 = ReadUlong(bytes[16..24], 8);
            var ul6 = ReadUlong(bytes[8..16], 8);
            var ul7 = ReadUlong(bytes[..8], 8);
            return new Int512(ul0, ul1, ul2, ul3, ul4, ul5, ul6, ul7);
        }

        private UInt512 ReadUInt512(Stream stream)
        {
            Span<byte> bytes = stackalloc byte[64];
            var bytesRead = ReadBytes(stream, ref bytes);
            AdjustBe(bytesRead, bytes, 32);
            var ul0 = ReadUlong(bytes[56..64], 8);
            var ul1 = ReadUlong(bytes[48..56], 8);
            var ul2 = ReadUlong(bytes[40..48], 8);
            var ul3 = ReadUlong(bytes[32..40], 8);
            var ul4 = ReadUlong(bytes[24..32], 8);
            var ul5 = ReadUlong(bytes[16..24], 8);
            var ul6 = ReadUlong(bytes[8..16], 8);
            var ul7 = ReadUlong(bytes[..8], 8);
            return new UInt512(ul0, ul1, ul2, ul3, ul4, ul5, ul6, ul7);
        }

        private static void AdjustBe(int bytesRead, Span<byte> bytes, int size)
        {
            if (bytesRead < size)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    bytes[size - i] = bytes[bytesRead - i];
                    bytes[bytesRead - i] = 0;
                }
            }
        }
    }
}