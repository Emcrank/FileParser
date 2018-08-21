using System.Collections.Generic;

namespace NeatParser
{
    /// <summary>
    /// Interface for the layout editor. Allows a column to edit the layout mid-parse.
    /// </summary>
    public interface ILayoutEditor
    {
        /// <summary>
        /// Returns a collection of columns after the edit logic has been applied.
        /// </summary>
        /// <param name="layout">The layout to be edited.</param>
        /// <param name="args">Argument string for the layout editor.</param>
        /// <returns></returns>
        IList<Column> Edit(Layout layout, string args);
    }
}