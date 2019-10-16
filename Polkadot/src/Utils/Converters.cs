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

    public static byte[] StringToByteArray(string hex)
        {
            if ((hex[0] == '0') && (hex[1] == 'x'))
            {
                hex = hex.Substring(2);
            }

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ByteArrayToString(byte[] data, int length = 0)
        {
            if (length == 0)
            {
                length = data.Length;
            }

            var rd = data.AsMemory().Slice(0, length).ToArray();

            return BitConverter.ToString(rd).Replace("-", "");
        }
    }
}
