using System;
using System.IO;
using System.Text;

namespace NeatParser
{
    public class VariableLengthSpace : ISpace
    {
        private readonly int maxLengthOfField;

        public VariableLengthSpace(int maxLengthOfField)
        {
            if(maxLengthOfField == 0)
                throw new ArgumentException("0 is not valid for the max length of a variable length space.");

            this.maxLengthOfField = maxLengthOfField;
        }

        public string SnipData(Layout layout, StringBuilder dataBuffer)
        {
            int lengthOfLengthField = maxLengthOfField.ToString().Length;

            try
            {
                int actualFieldLength = Convert.ToInt32(dataBuffer.Extract(0, lengthOfLengthField));
                return dataBuffer.Extract(0, actualFieldLength);
            }
            catch(Exception ex) when(ex is OverflowException || ex is FormatException || ex is ArgumentException)
            {
                throw new InvalidDataException("Unable to extract field from data buffer.", ex);
            }
        }
    }
}