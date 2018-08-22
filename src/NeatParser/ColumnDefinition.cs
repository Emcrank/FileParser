using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace NeatParser
{
    /// <summary>
    /// Base class for a column definition.
    /// </summary>
    /// <typeparam name="T">Type of column</typeparam>
    public class ColumnDefinition<T> : IColumnDefinition
    {
        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets a value whether or not this column is a dummy column.
        /// </summary>
        public virtual bool IsDummy { get; set; } = false;

        /// <summary>
        /// Gets or sets the IsLayoutEdito flag.
        /// </summary>
        public virtual bool IsLayoutEditor { get; } = false;

        /// <summary>
        /// Gets or sets the layout editor for the column.
        /// </summary>
        public ILayoutEditor LayoutEditor { get; protected set; }

        /// <summary>
        /// Gets the column metadata dictionary.
        /// </summary>
        public IDictionary Metadata { get; } = new Dictionary<object, object>();

        /// <summary>
        /// Gets or sets a value on whether or not the value should be trimmed after parsing before converting.
        /// </summary>
        public virtual TrimOptions TrimOption { get; set; } = TrimOptions.Trim;

        /// <summary>
        /// Constructs an instance of <see cref="ColumnDefinition{T}"/> with a random column name.
        /// </summary>
        public ColumnDefinition() : this(Path.GetRandomFileName()) { }

        /// <summary>
        /// Constructs an instance of <see cref="ColumnDefinition{T}"/> with the specified column name.
        /// </summary>
        /// <param name="columnName"></param>
        public ColumnDefinition(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException(nameof(columnName));

            ColumnName = columnName;
        }

        /// <summary>
        /// Parses the value given into object.
        /// </summary>
        /// <param name="value">Value to parse</param>
        /// <returns>Parsed value</returns>
        public object Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            string trimmedValue = PerformTrimming(value);
            return OnParse(trimmedValue);
        }

        /// <summary>
        /// Adds metadata to the metadata dictionary for this column.
        /// </summary>
        /// <param name="key">The key to add</param>
        /// <param name="value">The value to add</param>
        /// <returns>This column definition.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public IColumnDefinition AddMetadata(object key, object value)
        {
            Metadata.Add(key, value);
            return this;
        }

        /// <summary>
        /// Internal implementation of the Parse method.
        /// </summary>
        /// <param name="value">Value to parse</param>
        /// <returns>Parsed value</returns>
        /// <exception cref="NeatParserException">Throws this exception when issue occurs.</exception>
        protected virtual T OnParse(string value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex) when (ex is InvalidCastException || ex is FormatException)
            {
                return default(T);
            }
            catch (Exception ex) when (ex is OverflowException || ex is ArgumentNullException)
            {
                throw new NeatParserException(ex);
            }
        }

        private string PerformTrimming(string value)
        {
            switch (TrimOption)
            {
                case TrimOptions.Trim:
                    return value.Trim();

                case TrimOptions.LeftTrim:
                    return value.TrimLeading();

                case TrimOptions.RightTrim:
                    return value.TrimTrail();

                default:
                    return value;
            }
        }
    }
}