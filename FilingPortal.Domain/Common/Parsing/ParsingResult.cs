using System.Collections.Generic;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Represents Result of the parsing process
    /// </summary>
    /// <typeparam name="T">Type of the parsing data model</typeparam>
    public class ParsingResult<T>
    {
        /// <summary>
        /// The collection of the common errors
        /// </summary>
        private readonly List<string> _commonErrors;
        /// <summary>
        /// Gets a readonly collection of the common errors
        /// </summary>
        public IReadOnlyCollection<string> Errors => _commonErrors.AsReadOnly();
        /// <summary>
        /// The collection of the row parsing errors
        /// </summary>
        private readonly List<RowError> _rowErrors;
        /// <summary>
        /// Gets a readonly collection of the row parsing errors
        /// </summary>
        public IReadOnlyCollection<RowError> RowErrors => _rowErrors.AsReadOnly();
        /// <summary>
        /// Gets a collection of the parsed data
        /// </summary>
        public List<T> ParsedData { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingResult{T}"/> class
        /// </summary>
        /// <typeparam name="T">Type of the parsed data</typeparam>
        public ParsingResult()
        {
            ParsedData = new List<T>();
            _commonErrors = new List<string>();
            _rowErrors = new List<RowError>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingResult{T}"/> class with specified set of data
        /// </summary>
        /// <typeparam name="T">Type of the parsed data</typeparam>
        /// <param name="models">Set of parsed data</param>
        public ParsingResult(IEnumerable<T> models) : this()
        {
            ParsedData.AddRange(models);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingResult{T}"/> class with specified set of _commonErrors
        /// </summary>
        /// <typeparam name="T">Type of the parsed data</typeparam>
        /// <param name="errors">Set of the parsed data errors</param>
        public ParsingResult(IEnumerable<string> errors) : this()
        {
            this._commonErrors.AddRange(errors);
        }

        /// <summary>
        /// Adds a common parsing error to the error list
        /// </summary>
        /// <param name="error">The error message to add</param>
        public void AddError(string error)
        {
            _commonErrors.Add(error);
        }

        /// <summary>
        /// Adds a row parsing error
        /// </summary>
        /// <param name="error">Parsing row error to add</param>
        public void AddError(RowError error)
        {
            _rowErrors.Add(error);
        }
    }
}
