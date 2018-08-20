using System;
using System.ComponentModel;
using System.IO;

namespace FileParser
{
    public class LayoutEditorColumn : ColumnDefinition<string>
    {
        public override bool IsDummy => false;

        public override bool IsLayoutEditor => true;

        /// <summary>
        /// Constructs an instance of <see cref="LayoutEditorColumn"/> class with specified layout editor instance.
        /// </summary>
        public LayoutEditorColumn(ILayoutEditor layoutEditor) : this(Path.GetRandomFileName())
        {
            if(layoutEditor == null)
                throw new ArgumentNullException(nameof(layoutEditor));

            LayoutEditor = layoutEditor;
        }

        /// <summary>
        /// Constructs an instance of <see cref="LayoutEditorColumn"/> class with specified column name.
        /// </summary>
        /// <param name="columnName"></param>
        public LayoutEditorColumn(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException(nameof(columnName));

            ColumnName = columnName;
        }
    }
}