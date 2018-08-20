namespace NeatParser
{
    /// <summary>
    /// Extension class for string.
    /// </summary>
    internal static class StringExtensions
    {
        private static char[] WhitespaceCharacters => new[] { Tab, Space, CarriageReturn, NewLine };
        private const char CarriageReturn = '\r';
        private const char NewLine = '\n';
        private const char Space = ' ';
        private const char Tab = '\t';

        /// <summary>
        /// Trims whitespace from the end of a string.
        /// </summary>
        /// <param name="value">value to trim</param>
        /// <returns>Trimmed string.</returns>
        internal static string TrimLeading(this string value)
        {
            return value.TrimStart(WhitespaceCharacters);
        }

        /// <summary>
        /// Trims whitespace from the start of a string.
        /// </summary>
        /// <param name="value">value to trim</param>
        /// <returns>Trimmed string.</returns>
        internal static string TrimTrail(this string value)
        {
            return value.TrimEnd(WhitespaceCharacters);
        }
    }
}