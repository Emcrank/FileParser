using System.Collections.Generic;
using System.Linq;

namespace NeatParser
{
    /// <summary>
    /// Class that represents a container which allows the record values to be retrieved.
    /// </summary>
    public class RecordValueContainer
    {
        /// <summary>
        /// Gets the collection that exposes the record values.
        /// </summary>
        public IReadOnlyDictionary<string, object> RecordValues { get; }

        private readonly Layout layout;

        /// <summary>
        /// Gets the value that corresponds to the column.
        /// </summary>
        /// <param name="columnIndex">Index of the column</param>
        /// <returns>Dynamic value of specified column.</returns>
        public dynamic this[int columnIndex] => RecordValues.Values.ToList()[columnIndex];

        /// <summary>
        /// Gets the value that corresponds to the column.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <returns>Dynamic value of specified column.</returns>
        public dynamic this[string columnName] => layout[columnName].Definition.IsDummy ? null : RecordValues[columnName];

        internal RecordValueContainer(Layout layout, IReadOnlyDictionary<string, object> recordValues)
        {
            this.layout = layout;
            RecordValues = recordValues;
        }
    }
}