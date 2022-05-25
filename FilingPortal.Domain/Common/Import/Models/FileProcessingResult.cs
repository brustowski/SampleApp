using FilingPortal.Domain.Common.Parsing;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Common.Import.Models
{
    /// <summary>
    /// Represents file processing result
    /// </summary>
    public class FileProcessingResult
    {
        /// <summary>
        /// Gets or sets parsing errors
        /// </summary>
        public List<RowError> ParsingErrors { get; }
        /// <summary>
        /// Gets or sets validation errors
        /// </summary>
        public List<RowError> ValidationErrors { get; }
        /// <summary>
        /// Gets or sets common errors message
        /// </summary>
        public List<string> CommonErrors { get; }
        /// <summary>
        /// Gets or sets Name of the processing file
        /// </summary>
        public string FileName { get; }
        /// <summary>
        /// Determines whether there are no errors
        /// </summary>
        public bool IsValid => !(ValidationErrors.Any() || CommonErrors.Any() || ParsingErrors.Any());
        /// <summary>
        /// Gets or sets the number of the processed records
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcessingResult" /> class
        /// </summary>
        public FileProcessingResult(string fileName)
        {
            FileName = fileName;
            ParsingErrors = new List<RowError>();
            ValidationErrors = new List<RowError>();
            CommonErrors = new List<string>();
            Count = 0;
        }        
        /// <summary>
        /// Adds validation errors collection to the list of validation errors
        /// </summary>
        /// <param name="validationErrors">Validation errors collection to add</param>
        public void AddValidationErrors(IEnumerable<RowError> validationErrors)
        {
            if (validationErrors.Any())
            {
                ValidationErrors.AddRange(validationErrors);
            }
        }
        /// <summary>
        /// Adds validation errors collection to the list of validation errors
        /// </summary>
        /// <param name="errors">Validation errors collection to add</param>
        public void AddParsingErrors(IEnumerable<RowError> errors)
        {
            ParsingErrors.AddRange(errors);
        }
        /// <summary>
        /// Adds common file processing error to the list of common errors
        /// </summary>
        /// <param name="error">File processing error</param>
        public void AddCommonError(string error)
        {
            CommonErrors.Add(error);
        }
        /// <summary>
        /// Adds common file processing errors to the list of common errors
        /// </summary>
        /// <param name="errors">Set of file processing error</param>
        public void AddCommonErrors(IEnumerable<string> errors)
        {
            CommonErrors.AddRange(errors);
        }
    }
}
