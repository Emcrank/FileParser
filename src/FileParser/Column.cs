using System;

namespace FileParser
{
    /// <summary>
    /// Class to represent a column within a file.
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Gets a value that holds the definition for the column.
        /// </summary>
        public IColumnDefinition Definition { get; }

        /// <summary>
        /// Gets the layout that this column belongs to.
        /// </summary>
        public Layout Layout { get; }

        /// <summary>
        /// Gets a value that holds information about the size of the column.
        /// </summary>
        public ISpace Space { get; }

        /// <summary>
        /// Constructs a column instance.
        /// </summary>
        /// <param name="layout">layout</param>
        /// <param name="definition">Column definition for column</param>
        /// <param name="space">Space defined for column</param>
        internal Column(Layout layout, IColumnDefinition definition, ISpace space)
        {
            if (layout == null)
                throw new ArgumentNullException(nameof(layout));

            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (space == null)
                throw new ArgumentNullException(nameof(space));

            Layout = layout;
            Definition = definition;
            Space = space;
        }

        /// <summary>
        /// Parses the value given into object.
        /// </summary>
        /// <param name="value">Value to parse</param>
        /// <returns>Parsed value</returns>
        public object Parse(string value)
        {
            return Definition.Parse(value);
        }
    }
}