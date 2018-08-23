using System;
using System.Collections;
using System.Collections.Generic;

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
        IDictionary<string, object> Metadata { get; }

        /// <summary>
        /// Gets a value that determines if the column is a required field.
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        /// Parses text and converts if necessary.
        /// </summary>
        /// <param name="value">Text to be parsed.</param>
        /// <returns>Parsed value as object.</returns>
        object Parse(string value);

        /// <summary>
        /// Method to add metadata to the column definition.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>This column definition.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        IColumnDefinition AddMetadata(string key, object value);
    }
}