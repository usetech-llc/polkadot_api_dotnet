using System.Globalization;
using System.Runtime.InteropServices;

namespace Polkadot.Utils
{
    using System;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    public static class Converters
    {
        public static BigInteger FromHex(string hexStr, bool bigEndianBytes = false)
        {
            int offset = 0;
            int byteOffset = 0;
            if ((hexStr[0] == '0') && (hexStr[1] == 'x'))
            {
                offset = 2;
            }
            BigInteger result = 0;
            while (offset < hexStr.Length)
            {
                byte bt = FromHexByte(hexStr.Substring(offset, 2));
                if (bigEndianBytes)
                {
                    result = (result << 8) | bt;
                }
                else
                {
                    var wbyte = bt;
                    result = (wbyte << byteOffset) | result;
                    byteOffset += 8;
                }

                offset += 2;
            }
            return result;
        }

        public static byte FromHexByte(string byteStr) {
            char digit1 = byteStr[0];
                char digit2 = byteStr[1];
                byte bt = 0;
            if ((digit1 >= 'a') && (digit1 <= 'f'))
            {
                digit1 -= 'a';
                digit1 += (char)10;
            }
            else if ((digit1 >= 'A') && (digit1 <= 'F'))
            {
                digit1 -= 'A';
                digit1 += (char)10;
            }
            else if ((digit1 >= '0') && (digit1 <= '9'))
            {
                digit1 -= '0';
            }            
            if ((digit2 >= 'a') && (digit2 <= 'f'))
            {
                digit2 -= 'a';
                digit2 += (char)10;
            }
            else if ((digit2 >= 'A') && (digit2 <= 'F'))
            {
                digit2 -= 'A';
                digit1 += (char)10;
            }
            else if ((digit2 >= '0') && (digit2 <= '9'))
            {
                digit2 -= '0';
            }

            bt = (byte)((digit1 << 4) | digit2);
            return bt;
        }

        public static byte[] HexToByteArray(this string hex)
        {
            var span = hex.AsSpan();
            if ((hex[0] == '0') && (hex[1] == 'x'))
            {
                span = span[2..];
            }

            var value = new byte[span.Length / 2];
            for (int i = 0; i < value.Length; i++)
            {
                var substring = span[(i * 2)..(i * 2 + 2)];
                value[i] = byte.Parse(substring.ToString(), NumberStyles.HexNumber);
            }

            return value;
        }

        public static string ToHexString(this byte[] data, int? length = null)
        {
            if (length != null)
            {
                data = data.AsMemory().Slice(0, length.Value).ToArray();
            }

            return BitConverter.ToString(data).Replace("-", "");
        }

        public static string ToPrefixedHexString(this byte[] data, int? length = null)
        {
            return $"0x{data.ToHexString(length)}";
        }

        public static unsafe TResult ToStruct<TResult>(this byte[] array) where TResult : struct
        {
            fixed (byte* ptr = array)
            {
                 return Marshal.PtrToStructure<TResult>(new IntPtr(ptr));
            }
        }
    }
}
