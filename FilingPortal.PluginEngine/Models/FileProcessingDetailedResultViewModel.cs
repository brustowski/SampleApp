using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Represents file processing result view model
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, Name = "FileProcessingDetailedResultViewModelServer", Order = 1)]
    public class FileProcessingDetailedResultViewModel<TValidationError> : FileProcessingResultViewModel<TValidationError>
    {
        /// <summary>
        /// Gets or sets the number of the inserted records
        /// </summary>
        public int Inserted { get; set; }
        /// <summary>
        /// Gets or sets the number of the updated records
        /// </summary>
        public int Updated { get; set; }
        /// <summary>
        /// Gets or sets the payload
        /// </summary>
        public object Payload { get; set; }
    }
}