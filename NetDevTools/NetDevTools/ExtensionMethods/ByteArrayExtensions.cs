using System;
using System.Collections.Generic;
using System.Text;

namespace NetDevTools.ExtensionMethods
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Gets string from bytes buffer using default encoding of OS.
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
    }
}
