using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NetDevTools.Utils
{
    public static class PathUtils
    {
        /// <summary>
        /// Omits some parts of path in order to fit <param name="maxCharsCount">maxCharsCount</param> which is the max number of characters, not including special characters like slashes, ellipsis.
        /// </summary>
        /// <param name="longPath">Input path to be shortened.</param>
        /// <param name="maxCharsCount">Maximum number of path chars to be left, excluding special chars.</param>
        /// <returns>Shortened path.</returns>
        public static string ShortenLongPath(string longPath, int maxCharsCount)
        {
            var directorySeparator = Path.DirectorySeparatorChar;
            var ellipsis = "...";

            var folders = longPath.Split(directorySeparator);
            var last = folders.Last();
            var extension = Path.GetExtension(last);
            var hasExtension = !string.IsNullOrWhiteSpace(extension);

            var smallestWithExt = 8; // Example: 'P:\...\Fil...dll'.
            var smallestWithoutExt = 5; // Example: 'P:\...\Fil...'.

            var sb = new StringBuilder(maxCharsCount + ellipsis.Length * 3); // Just to get buffer bigger than expected shortened path.

            while(sb.Length <= maxCharsCount)

            if (maxCharsCount <= smallestWithExt)
            {


                if (maxCharsCount <= smallestWithoutExt)
                {

                }
            }


            if (last.Length <= maxCharsCount)
            {
                sb.Append(ellipsis);
                sb.Append(directorySeparator);

                if (string.IsNullOrWhiteSpace(extension))
                {
                    // No extension.
                    sb.Append(last.Substring(0, maxCharsCount));
                }
                else
                {
                    sb.Append(last.Substring(0, maxCharsCount - (extension.Length + 1)));
                    sb.Append(ellipsis);
                    sb.Append(extension.Substring(1));
                }
            }

            return sb.ToString();
        }
    }
}
