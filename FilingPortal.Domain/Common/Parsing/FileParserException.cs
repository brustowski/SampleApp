using System;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Represents errors that occur during file parsing process
    /// </summary>
    public class FileParserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileParserException"/> class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public FileParserException(string message) : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FileParserException"/> class with a specified error 
        /// message and a reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="innerException">
        /// <see cref="Exception"/> that is the cause of the current exception, or a null reference if no inner exception is specified
        /// </param>
        public FileParserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
