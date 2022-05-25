using FilingPortal.Domain.Services;

namespace FilingPortal.Domain.Common.Import
{
    /// <summary>
    /// Describes Template processing service factory
    /// </summary>
    public interface ITemplateProcessingServiceFactory
    {
        /// <summary>
        /// Create an instance of the Template processing service specified by configuration
        /// </summary>
        /// <param name="configuration"><see cref="IImportConfiguration"/></param>
        ITemplateProcessingService Create(IImportConfiguration configuration);

        /// <summary>
        /// Create an instance of the form data Template processing service specified by configuration
        /// </summary>
        /// <param name="configuration"><see cref="IFormImportConfiguration"/></param>
        IFormDataTemplateProcessingService Create(IFormImportConfiguration configuration);
    }
}
