namespace Polkadot.Source.Utils
{
    using System;

    public struct CompactInteger
    {
        public static CompactInteger operator +(CompactInteger self, int value)
        {
            self.Value += value;
            return self;
        }

        public static CompactInteger operator *(CompactInteger self, int value)
        {
            self.Value *= value;
            return self;
        }

        public static CompactInteger operator +(CompactInteger self, uint value)
        {
            self.Value += value;
            return self;
        }

        public static CompactInteger operator *(CompactInteger self, uint value)
        {
            self.Value *= value;
            return self;
        }

        public static CompactInteger operator +(CompactInteger self, byte value)
        {
            self.Value += value;
            return self;
        }

        public static CompactInteger operator *(CompactInteger self, byte value)
        {
            self.Value *= value;
            return self;
        }

        public static CompactInteger operator +(CompactInteger self, CompactInteger value)
        {
            self.Value += value.Value;
            return self;
        }

        public static CompactInteger operator *(CompactInteger self, CompactInteger value)
        {
            self.Value *= value.Value;
            return self;
        }

        public long Value { get; set; }
    }

    public static class Scale
    {
        private static Exception CompactIntegerException(string v)
        {
            throw new InvalidCastException(v);
        }

        public static byte NextByte(ref string stringStream)
        {
            var bt = byte.Parse(stringStream.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            stringStream = stringStream.Substring(2);
            return bt;
        }

        public static ushort NextWord(ref string stringStream)
        {
            var minor = stringStream.Substring(0, 2);
            stringStream = stringStream.Substring(2);
            var major = stringStream.Substring(0, 2);
            stringStream = stringStream.Substring(2);

            return ushort.Parse(major + minor, System.Globalization.NumberStyles.HexNumber);
        }

        public static string ExtractString(ref string stringStream, long length)
        {
            return ExtractString(ref stringStream, (int)length);
        }

        public static string ExtractString(ref string stringStream, int length)
        {
            string s = string.Empty;
            while (length > 0)
            {
                s += (char)NextByte(ref stringStream);
                length--;
            }
            return s;
        }

        public static CompactInteger DecodeCompactInteger(ref string stringStream)
        {
            uint first_byte = NextByte(ref stringStream);
            uint flag = (first_byte) & 0b00000011u;
            uint number = 0u;

            switch (flag)
            {
                case 0b00u:
                    {
                        number = first_byte >> 2;
                        break;
                    }

                case 0b01u:
                    {
                        uint second_byte = NextByte(ref stringStream);

                        number = ((uint)((first_byte) & 0b11111100u) + (uint)(second_byte) * 256u) >> 2;
                        break;
                    }

                case 0b10u:
                    {
                        number = first_byte;
                        uint multiplier = 256u;

                        for (var i = 0u; i < 3u; ++i)
                        {
                            number += NextByte(ref stringStream) * multiplier;
                            multiplier = multiplier << 8;
                        }
                        number = number >> 2;
                        break;
                    }

                case 0b11:
                    {
                        uint bytes_count = ((first_byte) >> 2) + 4u;
                        CompactInteger multiplier = new CompactInteger { Value = 1u };
                        CompactInteger value = new CompactInteger { Value = 0 };

                        // we assured that there are m more bytes,
                        // no need to make checks in a loop
                        for (var i = 0u; i < bytes_count; ++i)
                        {
                            value += multiplier * NextByte(ref stringStream);
                            multiplier *= 256u;
                        }

                        return value;
                    }

                default:
                    throw CompactIntegerException("CompactInteger decode error: unknown flag");
            }

            return new CompactInteger { Value = number };
        }

        //CompactIntegerLEBytes encodeCompactInteger(uint128 n) {

        //    CompactIntegerLEBytes b;
        //    memset(&b, 0, sizeof(b));

        //    if (n <= 63) {
        //        b.length = 1;
        //        b.bytes[0] = (uint8_t)(n << 2);
        //    } else if (n <= 0x3FFF) {
        //        b.length = 2;
        //        b.bytes[0] = (uint8_t)(((n & 0x3F) << 2) | 0x01);
        //        b.bytes[1] = (uint8_t)((n & 0xFC0) >> 6);
        //    } else if (n <= 0x3FFFFFFF) {
        //        b.length = 4;
        //        b.bytes[0] = (uint8_t)(((n & 0x3F) << 2) | 0x02);
        //        n >>= 6;
        //        for (int i = 1; i < 4; ++i) {
        //            b.bytes[i] = (uint8_t)(n & 0xFF);
        //            n >>= 8;
        //        }
        //    } else { // Big integer mode
        //        b.length = 1;
        //        int byteNum = 1;
        //        while (n) {
        //            b.bytes[byteNum++] = (uint8_t)(n & 0xFF);
        //            n >>= 8;
        //        }
        //        b.length = byteNum;
        //        b.bytes[0] = ((byteNum - 5) << 2) | 0x03;
        //    }

        //    return move(b);
        //}

        //long writeCompactToBuf(CompactIntegerLEBytes ci, uint8_t *buf) {
        //    for (int i = 0; i < ci.length; ++i)
        //        buf[i] = ci.bytes[i];
        //    return ci.length;
        //}


    }
}
