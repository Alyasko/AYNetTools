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
    }
}
