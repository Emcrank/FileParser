using System.Collections.Generic;

namespace FileParser
{
    /// <summary>
    /// Class that represents a container which allows the record values to be retrieved.
    /// </summary>
    public class RecordValueContainer
    {
        /// <summary>
        /// Gets the collection that exposes the record values.
        /// </summary>
        public IReadOnlyList<object> RecordValues { get; }

        private readonly Layout layout;

        /// <summary>
        /// Gets the value that corresponds to the column.
        /// </summary>
        /// <param name="columnIndex">Index of the column</param>
        /// <returns>Dynamic value of specified column.</returns>
        public dynamic this[int columnIndex] => RecordValues[columnIndex];

        /// <summary>
        /// Gets the value that corresponds to the column.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <returns>Dynamic value of specified column.</returns>
        public dynamic this[string columnName]
        {
            get
            {
                var column = layout[columnName];
                if (column.Definition.IsDummy)
                    return null;

                int index = layout.NonDummyColumns.IndexOf(column);
                return RecordValues[index];
            }
        }

        internal RecordValueContainer(Layout layout, IReadOnlyList<object> recordValues)
        {
            this.layout = layout;
            RecordValues = recordValues;
        }
    }
}