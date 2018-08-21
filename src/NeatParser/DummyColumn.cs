using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace NeatParser
{
    /// <summary>
    /// Class that represents a column that you do not care about parsing.
    /// </summary>
    public class DummyColumn : IColumnDefinition
    {
        /// <summary>
        /// Gets the column name.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets the IsDummy value.
        /// </summary>
        public bool IsDummy => true;

        /// <summary>
        /// Gets the IsLayoutEditor value.
        /// </summary>
        public bool IsLayoutEditor => false;

        /// <summary>
        /// Gets the layout editor.
        /// </summary>
        public ILayoutEditor LayoutEditor => null;

        /// <summary>
        /// Gets the metadata dictionary.
        /// </summary>
        public IDictionary Metadata => new Dictionary<object, object>();

        /// <summary>
        /// Gets the TrimValue value.
        /// </summary>
        public TrimOptions TrimOption => TrimOptions.None;

        /// <summary>
        /// Constructs an instance of <see cref="DummyColumn"/> with a random column name.
        /// </summary>
        public DummyColumn() : this(Path.GetRandomFileName()) { }

        /// <summary>
        /// Constructs an instance of <see cref="DummyColumn"/> with a specified column name.
        /// </summary>
        public DummyColumn(string columnName)
        {
            ColumnName = columnName;
        }

        [Obsolete]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object Parse(string value)
        {
            // Should never be called. Throw NotImplementedException to highlight coding error.
            throw new NotImplementedException();
        }
    }
}