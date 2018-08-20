using System.Text;

namespace NeatParser
{
    public class FixedLengthSpace : ISpace
    {
        private readonly int fieldLength;

        public FixedLengthSpace(int fieldLength)
        {
            this.fieldLength = fieldLength;
        }

        public string SnipData(Layout layout, StringBuilder dataBuffer)
        {
            return dataBuffer.Extract(0, fieldLength);
        }
    }
}