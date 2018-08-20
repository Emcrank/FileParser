using System.Collections.Generic;

namespace NeatParser
{
    public interface ILayoutEditor
    {
        /// <summary>
        /// Returns a collection of columns with the logic applied.
        /// </summary>
        /// <param name="allColumns"></param>
        /// <returns></returns>
        IList<Column> Edit(IList<Column> allColumns, string args);
    }
}