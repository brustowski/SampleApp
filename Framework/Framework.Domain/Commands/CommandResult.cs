using Framework.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Domain.Commands
{
    /// <summary>
    /// Represents controller action result
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// If property is unknown, N/A should be used
        /// </summary>
        private const string DefaultPropertyName = "N/A";

        /// <summary>
        /// Errors list
        /// </summary>
        private readonly List<CommandResultError> _errors = new List<CommandResultError>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult"/> class.
        /// </summary>
        public CommandResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult"/> class.
        /// </summary>
        /// <param name="businnessResult">Command result</param>
        public CommandResult(CommandResult businnessResult)
            : this(businnessResult.Errors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult"/> class.
        /// </summary>
        /// <param name="failures">List of errors</param>
        public CommandResult(IEnumerable<CommandResultError> failures) => _errors = failures.ToList();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult"/> class.
        /// </summary>
        /// <param name="errorText">Error message</param>
        public CommandResult(string errorText)
            : this(DefaultPropertyName, errorText)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult"/> class.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="errorText">Error message</param>
        /// <param name="attemptedValue">Value that probably caused error</param>
        public CommandResult(string propertyName, string errorText, object attemptedValue = null) => AddFailure(propertyName, errorText, attemptedValue);

        /// <summary>
        /// Creates empty valid result
        /// </summary>
        public static CommandResult Ok => new CommandResult();

        /// <summary>
        /// Gets the Errors list
        /// </summary>
        public IList<CommandResultError> Errors => _errors;

        /// <summary>
        /// Gets a value indicating whether command result is valid
        /// </summary>
        public bool IsValid => (_errors.Count == 0);

        /// <summary>
        /// Creates invalid result
        /// </summary>
        /// <param name="message">Error message</param>
        public static CommandResult Failed(string message) => new CommandResult(message);

        /// <summary>
        /// Adds failure to command result
        /// </summary>
        /// <param name="validationFailure">The <see cref="CommandResultError"/> failure</param>
        public void AddFailure(CommandResultError validationFailure)
        {
            Check.NotNull(validationFailure, "validationFailure");
            _errors.Add(validationFailure);
        }

        /// <summary>
        /// Adds failure to command result
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <param name="error">The error message</param>
        /// <param name="attemptedValue">The value that probably caused error</param>
        public void AddFailure(string propertyName, string error, object attemptedValue = null) => AddFailure(new CommandResultError(propertyName, error, attemptedValue));

        /// <summary>
        /// Merges command results
        /// </summary>
        /// <param name="validationResult">The command result to be merged with</param>
        public void Merge(CommandResult validationResult)
        {
            Check.NotNull(validationResult, "validationResult");
            Merge(validationResult.Errors);
        }

        /// <summary>
        /// Merges command result errors
        /// </summary>
        /// <param name="validationResult">Errors list</param>
        public void Merge(IEnumerable<CommandResultError> validationResult)
        {
            Check.NotNull(validationResult, "validationResult");
            foreach (CommandResultError error in validationResult)
            {
                _errors.Add(error);
            }
        }
    }

    /// <summary>
    /// Command result with value
    /// </summary>
    /// <typeparam name="TValue">Value type</typeparam>
    public class CommandResult<TValue> : CommandResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult{TValue}"/> class.
        /// </summary>
        public CommandResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult{TValue}"/> class.
        /// </summary>
        /// <param name="businnessResult">Original command result</param>
        public CommandResult(CommandResult businnessResult)
            : base(businnessResult.Errors)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult{TValue}"/> class.
        /// </summary>
        /// <param name="failures">The failures list</param>
        public CommandResult(IEnumerable<CommandResultError> failures)
            : base(failures)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult{TValue}"/> class.
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <param name="error">The error message</param>
        /// <param name="attemptedValue">The value that probably caused an error</param>
        public CommandResult(string propertyName, string error, object attemptedValue = null)
            : base(propertyName, error, attemptedValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult{TValue}"/> class.
        /// </summary>
        /// <param name="value">The value</param>
        public CommandResult(TValue value) => Value = value;

        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public TValue Value { get; set; }
    }

    /// <summary>
    /// Defines the failure in command result
    /// </summary>
    public class CommandResultError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResultError"/> class.
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <param name="error">The error message</param>
        public CommandResultError(string propertyName, string error)
        {
            PropertyName = propertyName;
            ErrorMessage = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResultError"/> class.
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <param name="error">The error message</param>
        /// <param name="data">Additional error data</param>
        public CommandResultError(string propertyName, string error, object data)
            : this(propertyName, error) => Data = data;

        /// <summary>
        /// Gets the additional data
        /// </summary>
        public object Data { get; private set; }

        /// <summary>
        /// Gets the Error Message
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Gets the Property Name
        /// </summary>
        public string PropertyName { get; private set; }
    }
}
