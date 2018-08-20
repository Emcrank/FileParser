using System;
using System.Collections.Generic;
using System.Linq;

namespace NeatParser
{
    public class Layout
    {
        /// <summary>
        ///  Used as a holding list for all the columns assigned at compile time.
        /// </summary>
        private readonly IList<Column> definedColumns = new List<Column>();

        /// <summary>
        /// Returns a copy of the columns which were assigned at compile time.
        /// </summary>
        public IList<Column> DefinedColumns => definedColumns.ToList();
        
        /// <summary>
        /// Gets a value of the total current columns defined in the layout.
        /// </summary>
        public int TotalCurrentColumns => CurrentColumns.Count;

        /// <summary>
        /// Gets a collection of column instances that make up this layout.
        /// </summary>
        internal IList<Column> CurrentColumns { get; private set; } = new List<Column>();

        /// <summary>
        /// Gets a collection of non dummy current column instances that make up this layout.
        /// </summary>
        internal IList<Column> NonDummyColumns => CurrentColumns.Where(c => !c.Definition.IsDummy).ToList();

        /// <summary>
        /// Gets a column from the CurrentColumns collection with the specified index.
        /// </summary>
        /// <param name="columnIndex">Index of column</param>
        /// <returns>Column at specified index</returns>
        internal Column this[int columnIndex] => CurrentColumns[columnIndex];

        /// <summary>
        /// Gets a column from the CurrentColumns collection with the specified column name.
        /// </summary>
        /// <param name="columnName">Name of column</param>
        /// <returns>Column with specified name</returns>
        internal Column this[string columnName] =>
            CurrentColumns.FirstOrDefault(c => c.Definition.ColumnName.Equals(columnName, StringComparison.Ordinal));

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
            CurrentColumns.Add(column);
            definedColumns.Add(column);
        }

        /// <summary>
        /// Edits the layout using the specified layout editor.
        /// </summary>
        /// <param name="editor">Layout editor</param>
        /// <param name="args">Arguments for the editor.</param>
        internal void Edit(ILayoutEditor editor, string args)
        {
            CurrentColumns = editor.Edit(this, args);
        }

        /// <summary>
        /// Resets the layout to as it was defined at compile time.
        /// </summary>
        internal void Reset()
        {
            CurrentColumns = DefinedColumns;
        }
    }
}