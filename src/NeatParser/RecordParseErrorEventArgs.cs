using System;

namespace NeatParser
{
    /// <summary>
    /// Event args for OnRecordRead event.
    /// </summary>
    public class RecordParseErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Exception that caused the error.
        /// </summary>
        public Exception Cause { get; }

        /// <summary>
        /// Holds the line that is read.
        /// </summary>
        public string LineData { get; }

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