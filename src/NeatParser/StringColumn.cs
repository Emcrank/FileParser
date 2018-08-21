namespace NeatParser
{
    /// <summary>
    /// Class for general string columns. Default column. Created for optimization.
    /// </summary>
    public class StringColumn : ColumnDefinition<string>
    {
        public override bool IsDummy => false;

        public override bool IsLayoutEditor => false;

        /// <summary>
        ///     Constructs a new instance of the <see cref="StringColumn" /> class.
        /// </summary>
        public StringColumn() { }

        /// <summary>
        ///     Constructs a new instance of the <see cref="StringColumn" /> class with specified column name.
        /// </summary>
        /// <param name="columnName">Name of the colum.</param>
        public StringColumn(string columnName) : base(columnName) { }

        protected override string OnParse(string value)
        {
            return value;
        }
    }
}