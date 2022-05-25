using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.Models.InboundRecordModels
{
    /// <summary>
    /// Defines the processing result for filing header, used in File and Save methods
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public class FilingResultViewModel
    {
        /// <summary>
        /// Gets or sets the error message
        /// </summary>
        public string ErrorMessage { get; set; } = null;

        /// <summary>
        /// Gets or sets the filingHeaderId
        /// </summary>
        public int FilingHeaderId { get; set; }

        /// <summary>
        /// Creates new good result for filing header
        /// </summary>
        /// <param name="filingHeaderId">The filingHeaderId</param>
        public FilingResultViewModel(int filingHeaderId)
        {
            FilingHeaderId = filingHeaderId;
        }

        /// <summary>
        /// Creates new bad result for filing header
        /// </summary>
        /// <param name="filingHeaderId">The filingHeaderId</param>
        /// <param name="errorMessage">The errorMessage</param>
        public FilingResultViewModel(int filingHeaderId, string errorMessage) : this(filingHeaderId)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets a value indicating whether result is valid
        /// </summary>
        public bool IsValid => string.IsNullOrEmpty(ErrorMessage);
    }
}
