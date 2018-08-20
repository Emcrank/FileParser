using System;
using System.Runtime.Serialization;

namespace NeatParser
{
    /// <summary>
    /// An exception thrown when an issue has occured during file parser process.
    /// </summary>
    [Serializable]
    public class FileParserException : Exception
    {
        private const string DefaultMessage = "An exception was thrown.";

        /// <summary>
        /// Constructs an instance of <see cref="FileParserException"/> with no message.
        /// </summary>
        public FileParserException()
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="FileParserException"/> with a specified message.
        /// </summary>
        /// <param name="message">Exception message</param>
        public FileParserException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="FileParserException"/> with a specified inner exception.
        /// </summary>
        /// <param name="innerException">Inner exception that caused the exception.</param>
        public FileParserException(Exception innerException) : base(DefaultMessage, innerException)
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="FileParserException"/> with a specified message and
        /// inner exception.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception that caused the exception.</param>
        public FileParserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="FileParserException"/> with a serialization info and
        /// streaming context.
        /// </summary>
        /// <param name="info">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        public FileParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}