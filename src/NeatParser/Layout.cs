using System;
using System.Collections.Generic;
using System.Linq;

namespace NeatParser
{
    public class Layout
    {
        /// <summary>
        /// Used as a holding list for all the columns assigned at compile time.
        /// </summary>
        private readonly IList<Column> allColumns = new List<Column>();

        /// <summary>
        /// Gets a value of the total columns defined in the layout.
        /// </summary>
        public int TotalColumns => Columns.Count;

        /// <summary>
        /// Gets a collection of column instances that make up this layout.
        /// </summary>
        internal IList<Column> Columns { get; private set; } = new List<Column>();

        /// <summary>
        /// Gets a collection of non dummy column instances that make up this layout.
        /// </summary>
        internal IList<Column> NonDummyColumns => Columns.Where(c => !c.Definition.IsDummy).ToList();

        /// <summary>
        /// Indexer for Columns
        /// </summary>
        /// <param name="columnIndex">Index of column</param>
        /// <returns>Column at specified index</returns>
        internal Column this[int columnIndex] => Columns[columnIndex];

        /// <summary>
        /// Indexer for Columns
        /// </summary>
        /// <param name="columnName">Name of column</param>
        /// <returns>Column with specified name</returns>
        internal Column this[string columnName] =>
            Columns.FirstOrDefault(c => c.Definition.ColumnName.Equals(columnName, StringComparison.Ordinal));

        /// <summary>
        /// Adds a column to the layout with the specified column definition and space.
        /// </summary>
        /// <param name="columnDefinition"></param>
        /// <param name="space"></param>
        public void AddColumn(IColumnDefinition columnDefinition, ISpace space)
        {
            if (columnDefinition == null)
                throw new ArgumentNullException(nameof(columnDefinition));

            if (space == null)
                throw new ArgumentNullException(nameof(space));

            if (this[columnDefinition.ColumnName] != null)
                throw new ArgumentException(
                    "You have already defined a column with this name. Column names must be unique per layout.");

            var column = new Column(this, columnDefinition, space);
            Columns.Add(column);
            allColumns.Add(column);
        }

        /// <summary>
        /// Edits the layout using the specified layout editor.
        /// </summary>
        /// <param name="editor">Layout editor</param>
        /// <param name="args">Arguments for the editor.</param>
        internal void Edit(ILayoutEditor editor, string args)
        {
            Columns = editor.Edit(allColumns, args);
        }

        /// <summary>
        /// Resets the layout to as it was defined at compile time.
        /// </summary>
        internal void Reset()
        {
            Columns = allColumns.ToList();
        }
    }
}