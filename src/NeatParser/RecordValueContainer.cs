using System.Collections.Generic;
using System.Linq;
using static System.FormattableString;

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
        ///     If the column definition in the layout has the property 'IsRequired' set to 'true' then this will throw an exception if the value is null/default(T).
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <returns>Dynamic value of specified column.</returns>
        /// <exception cref="NeatParserException">Thrown if no column was defined in the layout with this name.</exception>
        public dynamic this[string columnName] => GetValue(columnName);

        /// <summary>
        ///     The name of the layout used when the values were taken.
        /// </summary>
        public string LayoutName => layout.Name;

        private readonly Layout layout;

        internal RecordValueContainer(Layout correspondingLayout, IReadOnlyDictionary<string, object> recordValues)
        {
            layout = correspondingLayout;
            RecordValues = recordValues;
        }

        /// <summary>
        ///     Returns the value with the specided column name as the given type.
        /// </summary>
        /// <typeparam name="T">Type of Value</typeparam>
        /// <param name="columnName">Column name value to retrieve.</param>
        /// <returns>Value with the specided column name as the given type</returns>
        /// <exception cref="NeatParserException">
        ///     Thrown if column is not defined in layout or IsRequired property on column is set
        ///     to true and no value exists.
        /// </exception>
        public T Get<T>(string columnName)
        {
            var value = GetValue(columnName);
            return value != null ? (T)value : default(T);
        }

        private dynamic GetValue(string columnName)
        {
            var column = layout.DefinedColumns.FirstOrDefault(c => c.Definition.ColumnName.Equals(columnName));

            if(column == null)
                throw new NeatParserException(
                    Invariant($"No column was defined with the name '{columnName}' in the layout '{layout.Name}'."));

            dynamic value = RecordValues.ContainsKey(columnName) ? RecordValues[columnName] : null;
            bool isRequired = column.Definition.IsRequired;

            if(value == null && isRequired)
                throw new NeatParserException(Invariant(
                    $"The column '{columnName}' did not contain a value and has the IsRequired property set to 'true' defined in the layout '{layout.Name}'."));

            return value;
        }
    }
}