using System;
using System.IO;
using System.Text;

namespace NeatParser
{
    /// <summary>
    /// Class to represent a variable space within a file which is determined by a field length tag before it.
    /// </summary>
    public class VariableLengthSpace : ISpace
    {
        private readonly int maxLengthOfField;

        /// <summary>
        /// Constructs an instance of the <see cref="VariableLengthSpace"/> class with the specified max length of the field.
        /// </summary>
        /// <param name="maxLengthOfField"></param>
        public VariableLengthSpace(int maxLengthOfField)
        {
            if(maxLengthOfField == 0)
                throw new ArgumentException("0 is not valid for the max length of a variable length space.");

            this.maxLengthOfField = maxLengthOfField;
        }

        /// <summary>
        /// Snips down the dataBuffer and returns the string for this space.
        /// </summary>
        /// <param name="column">Column for the record</param>
        /// <param name="dataBuffer">Data buffer</param>
        /// <returns>The string data that belongs in this space.</returns>
        public string SnipData(Column column, StringBuilder dataBuffer)
        {
            int lengthOfLengthField = maxLengthOfField.ToString().Length;

            try
            {
                int actualFieldLength = Convert.ToInt32(dataBuffer.Extract(0, lengthOfLengthField));
                return dataBuffer.Extract(0, actualFieldLength);
            }
            catch(Exception ex) when(ex is OverflowException || ex is FormatException || ex is ArgumentException)
            {
                throw new FileParserException("Unable to extract field from data buffer.", ex);
            }
        }
    }
}