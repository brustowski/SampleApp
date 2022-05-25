namespace FilingPortal.Domain.Common.Import
{
    /// <summary>
    /// Describes import configuration registry
    /// </summary>
    public interface IImportConfigurationRegistry
    {
        /// <summary>
        /// Gets the import configuration
        /// </summary>
        /// <param name="name">Configuration name</param>
        IImportConfiguration GetConfiguration(string name);
    }
}
