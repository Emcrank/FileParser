using System;
using System.IO;

namespace NeatParser
{
    public class LayoutEditorColumn : ColumnDefinition<string>
    {
        public override bool IsDummy => false;

        /// <summary>
        ///     Constructs an instance of <see cref="LayoutEditorColumn" /> class with specified layout editor instance.
        ///     NOTE: This column must come before other columns that may be removed.
        /// </summary>
        public LayoutEditorColumn(ILayoutEditor layoutEditor) : this(Path.GetRandomFileName(), layoutEditor) { }

        /// <summary>
        ///     Constructs an instance of <see cref="LayoutEditorColumn" /> class with specified column name.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="layoutEditor"></param>
        public LayoutEditorColumn(string columnName, ILayoutEditor layoutEditor)
        {
            if(string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException(nameof(columnName));

            if(layoutEditor == null)
                throw new ArgumentNullException(nameof(layoutEditor));

            ColumnName = columnName;
            LayoutEditor = layoutEditor;
        }
    }
}