using System;
using System.Globalization;
using System.IO;

namespace NeatParser
{
    public class DateTimeColumn : ColumnDefinition<DateTime>
    {
        public override bool IsDummy => false;

        private readonly string format;

        /// <summary>
        ///     Constructs a new instance of the <see cref="DateTimeColumn" /> class with specified format.
        /// </summary>
        /// <param name="format">Format of the date time string e.g yyyyMMDD</param>
        /// <param name="isRequired">Is required</param>
        public DateTimeColumn(string format, bool isRequired = false) : this(Path.GetRandomFileName(), format, isRequired) { }

        /// <summary>
        ///     Constructs a new instance of the <see cref="DateTimeColumn" /> class with specified format and column name.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="format">Format of the date time string e.g yyyyMMDD</param>
        /// <param name="isRequired">Is required</param>
        public DateTimeColumn(string columnName, string format, bool isRequired = false) : base(columnName, isRequired)
        {
            if(string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException(nameof(columnName));

            if(string.IsNullOrWhiteSpace(format))
                throw new ArgumentNullException(nameof(format));

            this.format = format;
        }

        protected override DateTime OnParse(string value)
        {
            try
            {
                return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
            }
            catch(Exception ex) when(ex is FormatException || ex is ArgumentNullException)
            {
                throw new NeatParserException(ex);
            }
        }
    }
}