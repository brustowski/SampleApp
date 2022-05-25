using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Common.Parsing
{
    /// <summary>
    /// Represents parsed data validation Result
    /// </summary>
    public class ParsedDataValidationResult<TData> where TData : IParsingDataModel
    {
        /// <summary>
        /// Collection of valid data
        /// </summary>
        protected readonly List<TData> validData;
        public virtual IReadOnlyCollection<TData> ValidData => validData.AsReadOnly();
        /// <summary>
        /// Collection of <see cref="RowError"/>
        /// </summary>
        protected readonly List<RowError> errorsList;
        /// <summary>
        /// Gets <see cref="IReadOnlyCollection{T}"/> of row errors
        /// </summary>
        public IReadOnlyCollection<RowError> Errors => errorsList.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedDataValidationResult"/> class
        /// </summary>
        public ParsedDataValidationResult()
        {
            validData = new List<TData>();
            errorsList = new List<RowError>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedDataValidationResult"/> class with a specified collection of row errors
        /// </summary>
        /// <param name="errors"></param>
        public ParsedDataValidationResult(IEnumerable<RowError> errors): this()
        {
            errorsList.AddRange(errors);
        }
        
        /// <summary>
        /// Determines whether there are no errors 
        /// </summary>
        public bool IsValid => !errorsList.Any();

        /// <summary>
        /// Adds a new validation error
        /// </summary>
        /// <param name="error">Validation error to add</param>
        public void AddError(RowError error)
        {
            errorsList.Add(error);
        }        
        /// <summary>
        /// Adds validation errors from collection
        /// </summary>
        /// <param name="errors">Validation errors collection to add</param>
        public void AddError(IEnumerable<RowError> errors)
        {
            errorsList.AddRange(errors);
        }
        /// <summary>
        /// Adds a new valid data
        /// </summary>
        /// <param name="data">Data to add</param>
        public void AddData(TData data)
        {
            validData.Add(data);
        }
        /// <summary>
        /// Adds valid data from collection
        /// </summary>
        /// <param name="data">Valid data collection</param>
        public void AddData(IEnumerable<TData> data)
        {
            validData.AddRange(data);
        }
    }
}
