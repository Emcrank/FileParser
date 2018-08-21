using System.Collections;

namespace NeatParser
{
    /// <summary>
    /// Interface for column definition.
    /// </summary>
    public interface IColumnDefinition
    {
        /// <summary>
        /// Gets the column name.
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// Gets a value whether the column is just a dummy column.
        /// </summary>
        bool IsDummy { get; }

        /// <summary>
        /// Gets a value that determines if this column is a layout editor.
        /// </summary>
        bool IsLayoutEditor { get; }

        /// <summary>
        /// Gets the layout editor.
        /// </summary>
        ILayoutEditor LayoutEditor { get; }

        /// <summary>
        /// Gets the column metadata dictionary.
        /// </summary>
        IDictionary Metadata { get; }

        /// <summary>
        /// Parses text and converts if necessary.
        /// </summary>
        /// <param name="value">Text to be parsed.</param>
        /// <returns>Parsed value as object.</returns>
        object Parse(string value);
    }
}