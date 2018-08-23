using System;
using System.Collections.Generic;
using System.Linq;

namespace NeatParser
{
    /// <summary>
    ///     Class that represents a container which allows the record values to be retrieved.
    /// </summary>
    public class RecordValueContainer
    {
        /// <summary>
        ///     Gets the collection that exposes the record values.
        /// </summary>
        public IReadOnlyDictionary<string, object> RecordValues { get; }

        /// <summary>
        ///     Gets the value that corresponds to the column.
        /// </summary>
        /// <param name="columnIndex">Index of the column</param>
        /// <returns>Dynamic value of specified column.</returns>
        public dynamic this[int columnIndex] => columnIndex <= recordValuesList.Value.Count - 1
            ? recordValuesList.Value[columnIndex]
            : null;

        /// <summary>
        ///     Gets the value that corresponds to the column.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <returns>Dynamic value of specified column.</returns>
        public dynamic this[string columnName] =>
            RecordValues.ContainsKey(columnName) ? RecordValues[columnName] : null;

        /// <summary>
        /// The name of the layout used when the values were taken.
        /// </summary>
        public string LayoutName => layout.Name;

        private readonly Lazy<IList<object>> recordValuesList;
        private readonly Layout layout;

        internal RecordValueContainer(Layout correspondingLayout, IReadOnlyDictionary<string, object> recordValues)
        {
            layout = correspondingLayout;
            RecordValues = recordValues;
            recordValuesList = new Lazy<IList<object>>(() => RecordValues.Values.ToList());
        }
    }
}