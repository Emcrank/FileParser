using System;
using System.Runtime.Serialization;

namespace NeatParser
{
    /// <summary>
    /// An exception thrown when an issue has occured during file parser process.
    /// </summary>
    [Serializable]
    public class NeatParserException : Exception
    {
        private const string DefaultMessage = "An exception was thrown.";

        /// <summary>
        /// Constructs an instance of <see cref="NeatParserException"/> with no message.
        /// </summary>
        public NeatParserException()
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="NeatParserException"/> with a specified message.
        /// </summary>
        /// <param name="message">Exception message</param>
        public NeatParserException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="NeatParserException"/> with a specified inner exception.
        /// </summary>
        /// <param name="innerException">Inner exception that caused the exception.</param>
        public NeatParserException(Exception innerException) : base(DefaultMessage, innerException)
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="NeatParserException"/> with a specified message and
        /// inner exception.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception that caused the exception.</param>
        public NeatParserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructs an instance of <see cref="NeatParserException"/> with a serialization info and
        /// streaming context.
        /// </summary>
        /// <param name="info">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        public NeatParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}