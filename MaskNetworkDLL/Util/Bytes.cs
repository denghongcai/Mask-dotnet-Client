using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MaskGame.Util
{
    public static class Bytes
    {
        public static byte[] BitArrayToBytes(BitArray bits)
        {
            var bytes = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(bytes, 0);
            return bytes;
        }

        public static byte[] GetBEBytesFromNumber(uint num)
        {
            var numBytes = BitConverter.GetBytes(num);
            if(BitConverter.IsLittleEndian)
            {
                numBytes = numBytes.Reverse().ToArray();
            }

            return numBytes;
        }

        public static byte[] GetBEBytesFromNumber(long num)
        {
            var numBytes = BitConverter.GetBytes(num);
            if(BitConverter.IsLittleEndian)
            {
                numBytes = numBytes.Reverse().ToArray();
            }

            return numBytes;
        }

        public static long GetInt64FromBEBytes(byte[] bytes)
        {
            byte[] numBytes = bytes.Take(sizeof(long)).ToArray();
            if (BitConverter.IsLittleEndian)
            {
                numBytes = numBytes.Reverse().ToArray();
            }
            return BitConverter.ToInt64(numBytes, 0);
        }

        public static uint GetUInt32FromBEBytes(byte[] bytes)
        {
            byte[] numBytes = bytes.Take(sizeof(uint)).ToArray();
            if (BitConverter.IsLittleEndian)
            {
                numBytes = numBytes.Reverse().ToArray();
            }
            return BitConverter.ToUInt32(numBytes, 0);
        }

        public static uint GetUInt8FromBEBytes(byte[] bytes)
        {
            return bytes[0];
        }

        public static byte[] PaddingBytes(byte[] bytes, int length)
        {
            var paddedBytes = new byte[length];
            Buffer.BlockCopy(bytes, 0, paddedBytes, 0, bytes.Length);
            return paddedBytes;
        }

        public static string ReadStringUntil(byte[] b, char delimiter)
        {
            StringBuilder s = new StringBuilder();
            for(int i = 0; i < b.Length; i++)
            {
                try
                {
                    char c = (char)b[i];
                    if (c == delimiter) break;
                    else
                    {
                        s.Append(c);
                    }
                }
                catch(System.Exception)
                {
                    break;
                }
            }

            return s.ToString();
        }
    }
}
