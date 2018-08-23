using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeatParser
{
    public class Layout
    {
        /// <summary>
        /// Gets the layout name.
        /// </summary>
        public string Name { get; }

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
        public int TotalCurrentColumns => currentColumns.Count;

        /// <summary>
        /// Gets a collection of column instances that make up this layout.
        /// </summary>
        private IList<Column> currentColumns = new List<Column>();

        /// <summary>
        /// Returns a copy of the columns which are currently assigned to the layout.
        /// </summary>
        public IList<Column> CurrentColumns => currentColumns.ToList();

        /// <summary>
        /// Gets a column from the CurrentColumns collection with the specified index.
        /// </summary>
        /// <param name="columnIndex">Index of column</param>
        /// <returns>Column at specified index</returns>
        internal Column this[int columnIndex] => currentColumns[columnIndex];

        /// <summary>
        /// Gets a column from the CurrentColumns collection with the specified column name.
        /// </summary>
        /// <param name="columnName">Name of column</param>
        /// <returns>Column with specified name</returns>
        internal Column this[string columnName] =>
            currentColumns.FirstOrDefault(c => c.Definition.ColumnName.Equals(columnName, StringComparison.Ordinal));

        /// <summary>
        /// Initializes a <see cref="Layout"/> instance.
        /// </summary>
        public Layout() : this(string.Concat("Layout_", Path.GetRandomFileName())) { }

        /// <summary>
        /// Initializes a <see cref="Layout"/> instance with specified layout name.
        /// </summary>
        /// <param name="layoutName">The name for the layout.</param>
        public Layout(string layoutName)
        {
            Name = layoutName;
        }

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
            currentColumns.Add(column);
            definedColumns.Add(column);
        }

        /// <summary>
        /// Edits the layout using the specified layout editor.
        /// </summary>
        /// <param name="editor">Layout editor</param>
        /// <param name="args">Arguments for the editor.</param>
        internal void Edit(ILayoutEditor editor, string args)
        {
            currentColumns = editor.Edit(this, args);
        }

        /// <summary>
        /// Resets the layout to as it was defined at compile time.
        /// </summary>
        internal void Reset()
        {
            currentColumns = DefinedColumns;
        }
    }
}