using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.FormattableString;

namespace NeatParser
{
    public class Layout
    {
        /// <summary>
        ///     Gets the layout name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the delimiter for columns.
        /// </summary>
        public string ColumnDelimiter { get; private set; }

        /// <summary>
        ///     Returns a copy of the columns which were assigned at compile time.
        /// </summary>
        public IList<Column> DefinedColumns => definedColumns.ToList();

        /// <summary>
        ///     Gets a value of the total current columns defined in the layout.
        /// </summary>
        public int TotalCurrentColumns => currentColumns.Count;

        /// <summary>
        ///     Returns a copy of the columns which are currently assigned to the layout.
        /// </summary>
        public IList<Column> CurrentColumns => currentColumns.ToList();

        /// <summary>
        ///     Gets a column from the CurrentColumns collection with the specified index.
        /// </summary>
        /// <param name="columnIndex">Index of column</param>
        /// <returns>Column at specified index</returns>
        internal Column this[int columnIndex] => currentColumns[columnIndex];

        /// <summary>
        ///     Gets a column from the CurrentColumns collection with the specified column name.
        /// </summary>
        /// <param name="columnName">Name of column</param>
        /// <returns>Column with specified name</returns>
        internal Column this[string columnName] =>
            currentColumns.FirstOrDefault(c => c.Definition.ColumnName.Equals(columnName, StringComparison.Ordinal));

        /// <summary>
        ///     Gets a value that reflects if the layout is delimited.
        /// </summary>
        internal bool IsDelimited => !string.IsNullOrEmpty(ColumnDelimiter);

        /// <summary>
        ///     Flag that lets the layout know that the last column contains a delimiter after it.
        ///     e.g "COLUMN1,COLUMN2,COLUMN3,LASTCOLUMN," vs "COLUMN1,COLUMN2,COLUMN3,LASTCOLUMN"
        /// </summary>
        public bool RecordsHaveTrailingDelimiter { get; set; } = true;

        /// <summary>
        ///     Used as a holding list for all the columns assigned at compile time.
        /// </summary>
        private readonly IList<Column> definedColumns = new List<Column>();

        /// <summary>
        ///     Gets a collection of column instances that make up this layout.
        /// </summary>
        private IList<Column> currentColumns = new List<Column>();

        /// <summary>
        ///     Initializes a <see cref="Layout" /> instance.
        /// NOTE: A new instance of Layout should be used for each instance of NeatParser.
        /// </summary>
        public Layout() : this(string.Concat("Layout_", Path.GetRandomFileName())) { }

        /// <summary>
        ///     Initializes a <see cref="Layout" /> instance with specified layout name.
        /// NOTE: A new instance of Layout should be used for each instance of NeatParser.
        /// </summary>
        /// <param name="layoutName">The name for the layout.</param>
        /// <param name="columnDelimiter">Optional delimiter which columns are delimited by (Applies to all columns).</param>
        public Layout(string layoutName, string columnDelimiter = null)
        {
            Name = layoutName;

            if(columnDelimiter != null)
                SetDelimiter(columnDelimiter);
        }

        /// <summary>
        ///     Adds a column to the layout with the specified column definition and space.
        /// </summary>
        /// <param name="columnDefinition"></param>
        /// <param name="space"></param>
        public void AddColumn(IColumnDefinition columnDefinition, ISpace space)
        {
            if(columnDefinition == null)
                throw new ArgumentNullException(nameof(columnDefinition));

            if(space == null)
                throw new ArgumentNullException(nameof(space));

            if(this[columnDefinition.ColumnName] != null)
                throw new ArgumentException(
                    "You have already defined a column with this name. Column names must be unique per layout.");

            AddColumn(new Column(this, columnDefinition, space));
        }

        private void AddColumn(Column column)
        {
            currentColumns.Add(column);
            definedColumns.Add(column);

            if(IsDelimited)
                AddDelimiterColumn(ColumnDelimiter);
        }

        /// <summary>
        ///     Sets the delimiter for the layout.
        ///     By setting this the layout will expect every column to be delimited by this string.
        /// </summary>
        /// <param name="delimiter"></param>
        public void SetDelimiter(string delimiter)
        {
            if(string.IsNullOrEmpty(delimiter))
                throw new ArgumentNullException(nameof(delimiter));

            if(!string.IsNullOrEmpty(ColumnDelimiter))
                throw new InvalidOperationException(Invariant(
                    $"You have already set a column delimiter for this layout. ColumnDelimiter: '{ColumnDelimiter}'"));

            if(definedColumns.Any())
                throw new InvalidOperationException(
                    "You must call to the SetDelimiter(string delimiter) method before adding any columns.");

            ColumnDelimiter = delimiter;
        }

        /// <summary>
        ///     Edits the layout using the specified layout editor.
        /// </summary>
        /// <param name="editor">Layout editor</param>
        /// <param name="args">Arguments for the editor.</param>
        internal void Edit(ILayoutEditor editor, string args)
        {
            currentColumns = editor.Edit(this, args);
        }

        /// <summary>
        ///     Resets the layout to as it was defined at compile time.
        /// </summary>
        internal void Reset()
        {
            currentColumns = DefinedColumns;

            if(!RecordsHaveTrailingDelimiter)
                currentColumns.RemoveAt(currentColumns.Count - 1);
        }

        /// <summary>
        ///     Adds a delimiter column.
        ///     <code>NOTE:</code> If every column is delimited by the same delimiter then use the <see cref="Layout" />.
        ///     <see cref="SetDelimiter" /> method instead.
        /// </summary>
        public void AddDelimiterColumn(string delimiter)
        {
            if(string.IsNullOrEmpty(delimiter))
                throw new ArgumentNullException(nameof(delimiter));

            var delimiterColumnDefinition = new DelimiterColumn(delimiter);
            var column = new Column(this, delimiterColumnDefinition, delimiterColumnDefinition.GetSpace());
            currentColumns.Add(column);
            definedColumns.Add(column);
        }
    }
}