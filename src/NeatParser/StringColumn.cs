using System.IO;

namespace NeatParser
{
    /// <summary>
    ///     Class for general string columns. Default column. Created for optimization.
    /// </summary>
    public class StringColumn : ColumnDefinition<string>
    {
        public override bool IsDummy => false;

        /// <summary>
        ///     Constructs a new instance of the <see cref="StringColumn" /> class.
        /// </summary>
        public StringColumn(bool isRequired = false) : base(Path.GetRandomFileName(), isRequired) { }

        /// <summary>
        ///     Constructs a new instance of the <see cref="StringColumn" /> class with specified column name.
        /// </summary>
        /// <param name="columnName">Name of the colum.</param>
        /// <param name="isRequired">Is required</param>
        public StringColumn(string columnName, bool isRequired = false) : base(columnName, isRequired) { }

        protected override string OnParse(string value)
        {
            return value;
        }
    }
}