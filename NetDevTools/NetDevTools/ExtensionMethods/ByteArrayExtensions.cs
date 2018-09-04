using System;
using System.Collections.Generic;
using System.Text;

namespace NetDevTools.ExtensionMethods
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Gets string from bytes buffer using default code page of OS.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string GetString(this byte[] buffer)
        {
            return Encoding.Default.GetString(buffer);
        }

        /// <summary>
        /// Gets string from bytes buffer using Encoding.Unicode encoding.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string GetStringUnicode(this byte[] buffer)
        {
            return Encoding.Unicode.GetString(buffer);
        }

        /// <summary>
        /// Converts bytes buffer to default code page string considering zero bytes.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string DecodeString(this byte[] buffer)
        {
            var count = Array.IndexOf<byte>(buffer, 0, 0);
            if (count < 0) count = buffer.Length;
            return Encoding.Default.GetString(buffer, 0, count);
        }

        /// <summary>
        /// Creates new array which size equals to current size plus size of array length.
        /// Inserts the UInt32 length of array into the first bytes of array.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] PrefixArraySize(this byte[] buffer)
        {
            var dataLength = (UInt32)buffer.Length;
            var newBuffer = new byte[sizeof(UInt32) + dataLength];
            var dataLengthBytes = BitConverter.GetBytes(dataLength);

            Buffer.BlockCopy(dataLengthBytes, 0, buffer, 0, sizeof(UInt32));
            Buffer.BlockCopy(buffer, 0, newBuffer, sizeof(UInt32), buffer.Length);

            return newBuffer;
        }
    }
}
