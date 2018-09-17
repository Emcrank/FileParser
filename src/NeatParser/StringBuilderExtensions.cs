using System;
using System.Text;

namespace NeatParser
{
    /// <summary>
    /// Extension class for string builder.
    /// </summary>
    internal static class StringBuilderExtensions
    {
        /// <summary>
        /// Extracts from stringbuilder and returns the result. The extracted string is also removed
        /// from the builder.
        /// </summary>
        /// <param name="sb">StringBuilder from which to extract.</param>
        /// <param name="startIndex">Starting index to extract from.</param>
        /// <param name="count">Amount of characters to extract.</param>
        /// <returns>Extracted string</returns>
        /// <exception cref="ArgumentNullException">Thrown when argument null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when arument out of range.</exception>
        /// <exception cref="ArgumentException">Other issues with argument.</exception>
        internal static string Extract(this StringBuilder sb, int startIndex, int count)
        {
            if (sb == null)
                throw new ArgumentNullException(nameof(sb));

            if (string.IsNullOrEmpty(sb.ToString()))
                throw new ArgumentNullException("The string builder contents were null or empty. Contents='" + sb + "'");

            if (startIndex < 0)
                throw new ArgumentException(nameof(startIndex));

            if (count <= 0)
                throw new ArgumentException(nameof(count));

            var charArray = new char[count];
            sb.CopyTo(0, charArray, 0, count);
            sb.Remove(0, count);
            return new string(charArray);
        }
    }
}