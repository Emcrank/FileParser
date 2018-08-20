using System;

namespace FileParser
{
    /// <summary>
    /// Options class for the File Reader.
    /// </summary>
    public class SeperatedRecordParserOptions
    {
        /// <summary>
        /// Gets or sets the string that defines the seperator for the records. Default = Environment.NewLine.
        /// </summary>
        public string RecordSeperator { get; set; } = Environment.NewLine;

        /// <summary>
        /// Gets or sets the amount of records to skip at the start of the read. Default = 0.
        /// </summary>
        public int SkipFirst { get; set; } = 0;
    }
}