using System;
using System.Collections.Generic;
using System.IO;
using static System.FormattableString;

namespace NeatParser
{
    /// <summary>
    ///     Base class for a column definition.
    /// </summary>
    /// <typeparam name="T">Type of column</typeparam>
    public class ColumnDefinition<T> : IColumnDefinition
    {
        /// <summary>
        ///     Gets or sets a value on whether or not the value should be trimmed after parsing before converting.
        /// </summary>
        public virtual TrimOptions TrimOption { get; set; } = TrimOptions.Trim;

        /// <summary>
        ///     Constructs an instance of <see cref="ColumnDefinition{T}" /> with a random column name.
        /// </summary>
        public ColumnDefinition() : this(Path.GetRandomFileName()) { }

        /// <summary>
        ///     Constructs an instance of <see cref="ColumnDefinition{T}" /> with the specified column name.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="isRequired">
        ///     Flag that determines if the value is required. A <see cref="NeatParserException" /> will be
        ///     thrown when trying to retrieve this value from a <see cref="RecordValueContainer" /> if it does not exist.
        /// </param>
        public ColumnDefinition(string columnName, bool isRequired = false)
        {
            if(string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException(nameof(columnName));

            ColumnName = columnName;
            IsRequired = isRequired;
        }

        /// <summary>
        ///     Gets or sets the column name.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///     Gets or sets a value whether or not this column is a dummy column.
        /// </summary>
        public virtual bool IsDummy { get; set; } = false;

        /// <summary>
        ///     Gets the IsLayoutEditor flag.
        /// </summary>
        public bool IsLayoutEditor => LayoutEditor != null;

        /// <summary>
        ///     Gets or sets the layout editor for the column.
        /// </summary>
        public ILayoutEditor LayoutEditor { get; protected set; }

        /// <summary>
        ///     Gets the column metadata dictionary.
        /// </summary>
        public IDictionary<string, object> Metadata { get; } = new Dictionary<string, object>();

        /// <summary>
        ///     Gets or sets a value that determines if the column is required to contain a value.
        ///     If no value is contained within the <see cref="RecordValueContainer" /> for this column
        ///     then a <see cref="NeatParserException" /> will be thrown.
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        ///     Parses the value given into object.
        /// </summary>
        /// <param name="value">Value to parse</param>
        /// <returns>Parsed value</returns>
        public object Parse(string value)
        {
            if(string.IsNullOrEmpty(value))
                return null;

            string trimmedValue = PerformTrimming(value);
            return OnParse(trimmedValue);
        }

        /// <summary>
        ///     Adds metadata to the metadata dictionary for this column.
        /// </summary>
        /// <param name="key">The key to add</param>
        /// <param name="value">The value to add</param>
        /// <returns>This column definition.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public IColumnDefinition AddMetadata(string key, object value)
        {
            Metadata.Add(key, value);
            return this;
        }

        /// <summary>
        ///     Internal implementation of the Parse method.
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
            catch(Exception ex) when(ex is InvalidCastException || ex is FormatException)
            {
                return default(T);
            }
            catch(Exception ex) when(ex is OverflowException || ex is ArgumentNullException)
            {
                throw new NeatParserException(Invariant($"An error occured when trying to convert the value '{value}' to type '{typeof(T).FullName}'."), ex);
            }
        }

        private string PerformTrimming(string value)
        {
            switch(TrimOption)
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