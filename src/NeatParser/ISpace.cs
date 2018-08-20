using System.Text;

namespace NeatParser
{
    /// <summary>
    /// Interface for space. Represents a horizontal space within a record.
    /// </summary>
    public interface ISpace
    {
        /// <summary>
        /// Returns the data that belongs to this space and removes it from the data buffer.
        /// </summary>
        /// <param name="column">Column for the record.</param>
        /// <param name="dataBuffer">StringBuilder containing data.</param>
        /// <returns>Returns the data that belongs to this space</returns>
        /// <exception cref="FileParserException">Thrown if something goes wrong.</exception>
        string SnipData(Column column, StringBuilder dataBuffer);
    }
}