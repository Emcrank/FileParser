using System;
using System.IO;
using System.Text;
using static System.FormattableString;

namespace NeatParser
{
    /// <summary>
    /// Represents a fixed width space within the file.
    /// </summary>
    public class FixedLengthSpace : ISpace
    {
        private readonly int fieldLength;

        /// <summary>
        /// Constructs an instance of the <see cref="FixedLengthSpace"/> class with specified field length.
        /// </summary>
        /// <param name="fieldLength"></param>
        public FixedLengthSpace(int fieldLength)
        {
            this.fieldLength = fieldLength;
        }

        /// <summary>
        /// Snips down the dataBuffer and returns the string for this space.
        /// </summary>
        /// <param name="column">Column for the record</param>
        /// <param name="dataBuffer">Data buffer</param>
        /// <returns>The string data that belongs in this space.</returns>
        public string SnipData(Column column, StringBuilder dataBuffer)
        {
            try
            {
                return dataBuffer.Extract(0, fieldLength);
            }
            catch (Exception ex) when (ex is OverflowException || ex is FormatException || ex is ArgumentException)
            {
                string message =
                    Invariant(
                        $"Unable to extract field from data buffer.\r\nData Buffer='{dataBuffer.ToString()}'\r\nFieldLength='{fieldLength}'");
                throw new NeatParserException(message, ex);
            }
        }
    }
}