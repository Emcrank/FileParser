using System;

namespace NeatParser
{
    /// <summary>
    /// Event args for OnRecordRead event.
    /// </summary>
    public class RecordReadEventArgs : EventArgs
    {
        /// <summary>
        /// Holds the line that is read.
        /// </summary>
        public string LineData { get; }

        /// <summary>
        /// Gets or sets a boolean that tells the reader whether the line should be skipped or not.
        /// </summary>
        public bool ShouldSkip { get; set; } = false;

        /// <summary>
        /// Constructor for RecordReadEventArgs.
        /// </summary>
        /// <param name="lineData">Line as string</param>
        internal RecordReadEventArgs(string lineData)
        {
            LineData = lineData;
        }
    }
}