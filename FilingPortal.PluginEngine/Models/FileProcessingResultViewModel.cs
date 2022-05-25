using System.Collections.Generic;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Represents file processing result view model
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, Name = "FileProcessingResultViewModelServer")]
    public class FileProcessingResultViewModel<TValidationError>
    {
        /// <summary>
        /// Gets or sets parsing errors
        /// </summary>
        public IEnumerable<TValidationError> ParsingErrors { get; set; }
        /// <summary>
        /// Gets or sets validation errors
        /// </summary>
        public IEnumerable<TValidationError> ValidationErrors { get; set; }
        /// <summary>
        /// Gets or sets common errors message
        /// </summary>
        public IEnumerable<string> CommonErrors { get; set; }
        /// <summary>
        /// Gets or sets Name of the processing file
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the number of the processed records
        /// </summary>
        public int Count { get; set; }
    }
}