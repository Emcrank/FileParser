using System.Text;

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
        /// <param name="layout">Layout</param>
        /// <param name="dataBuffer">Data buffer</param>
        /// <returns>The string data that belongs in this space.</returns>
        public string SnipData(Layout layout, StringBuilder dataBuffer)
        {
            return dataBuffer.Extract(0, fieldLength);
        }
    }
}