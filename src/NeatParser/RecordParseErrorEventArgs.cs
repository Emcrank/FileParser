using System;

namespace NeatParser
{
    /// <summary>
    /// Event args for OnRecordRead event.
    /// </summary>
    public class RecordParseErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the exception which is the cause of the issue.
        /// </summary>
        public Exception Cause { get; }

        /// <summary>
        /// Gets the line that parser last read.
        /// </summary>
        public string LineData { get; }

        /// <summary>
        /// Gets or sets the value that determines whether the parser
        /// should rethrow the exception or whether the user has handled it.
        /// </summary>
        public bool UserHandled { get; set; }

        /// <summary>
        /// Constructor for RecordParseErrorEventArgs.
        /// </summary>
        /// <param name="lineData">Line as string</param>
        /// <param name="ex">Exception that was the cause of the error.</param>
        internal RecordParseErrorEventArgs(string lineData, Exception ex)
        {
            LineData = lineData;
            Cause = ex;
        }
    }
}