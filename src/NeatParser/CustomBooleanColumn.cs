using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeatParser
{
    public class CustomBooleanColumn : ColumnDefinition<bool>
    {
        private readonly IList<string> trueValues;

        /// <summary>
        /// Constructs a new instance of <see cref="CustomBooleanColumn"/> with a random column name.
        /// </summary>
        /// <param name="trueValues"></param>
        public CustomBooleanColumn(string trueValues) : this(Path.GetRandomFileName(), trueValues) { }

        /// <summary>
        /// Constructs a new instance of <see cref="CustomBooleanColumn"/> with specified column name.
        /// </summary>
        /// <param name="columnName">Column names</param>
        /// <param name="trueValues">
        /// String with comma delimited values which should be considered as a true result.
        /// </param>
        public CustomBooleanColumn(string columnName, string trueValues) : base(columnName)
        {
            if (trueValues == null)
                throw new ArgumentNullException(nameof(columnName));

            this.trueValues = trueValues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// Method to convert the string value to the specified type.
        /// </summary>
        /// <param name="value">Value to be parsed</param>
        /// <returns>Converted value</returns>
        protected override bool OnParse(string value)
        {
            return trueValues.Any(x => value.Equals(x, StringComparison.Ordinal));
        }
    }
}