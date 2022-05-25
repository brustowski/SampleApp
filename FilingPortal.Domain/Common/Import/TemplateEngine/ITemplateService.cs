namespace FilingPortal.Domain.Common.Import.TemplateEngine
{
    /// <summary>
    /// Provides methods for creating templates
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// Creates the template file based on provided configuration
        /// </summary>
        /// <param name="configuration">Template configuration</param>
        FileExportResult Create(IImportConfiguration configuration);
    }
}
