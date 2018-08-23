using System;
using System.IO;

namespace NeatParser
{
    /// <summary>
    ///     Represents a delimiter in the record.
    /// </summary>
    public sealed class DelimiterColumn : DummyColumn
    {
        /// <summary>
        ///     Gets the delimiter.
        /// </summary>
        internal string Delimiter { get; }

        /// <summary>
        ///     Initializes a <see cref="DelimiterColumn" /> instance with the specified delimiter.
        /// </summary>
        /// <param name="delimiter"></param>
        public DelimiterColumn(string delimiter) : base("Delimiter_" + Path.GetRandomFileName())
        {
            if(string.IsNullOrEmpty(delimiter))
                throw new ArgumentNullException(nameof(delimiter));

            Delimiter = delimiter;
        }

        /// <summary>
        ///     Returns a fixed length spacing for this column.
        /// </summary>
        /// <returns></returns>
        internal ISpace GetSpace()
        {
            return new FixedLengthSpace(Delimiter.Length);
        }
    }
}