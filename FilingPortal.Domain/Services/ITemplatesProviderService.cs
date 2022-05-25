namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Provides information about templates stored in application
    /// </summary>
    public interface ITemplatesProviderService
    {
        /// <summary>
        /// Provider templates full path
        /// </summary>
        string GetTemplatesFolder();

        /// <summary>
        /// Provides specified template as an array of bytes
        /// </summary>
        /// <param name="templateName">The name of the template</param>
        byte[] GetTemplateAsByteArray(string templateName);
    }
}
