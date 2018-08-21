using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeatParser
{
    public class DateTimeColumn : ColumnDefinition<DateTime>
    {
        public override bool IsDummy => false;

        public override bool IsLayoutEditor => false;

        private readonly string format;

        /// <summary>
        /// Constructs a new instance of the <see cref="DateTimeColumn"/> class with specified format.
        /// </summary>
        /// <param name="format">Format of the date time string e.g yyyyMMDD</param>
        public DateTimeColumn(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new ArgumentNullException(nameof(format));

            this.format = format;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="DateTimeColumn"/> class with specified format and column name.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="format">Format of the date time string e.g yyyyMMDD</param>
        public DateTimeColumn(string columnName, string format) : base(columnName)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new ArgumentNullException(nameof(format));

            this.format = format;
        }

        protected override DateTime OnParse(string value)
        {
            try
            {
                return DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);
            }
            catch (Exception ex) when (ex is FormatException || ex is ArgumentNullException)
            {
                throw new FileParserException(ex);
            }
        }
    }
}
