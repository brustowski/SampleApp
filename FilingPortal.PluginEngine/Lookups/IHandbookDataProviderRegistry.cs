using System.Collections.Generic;

namespace FilingPortal.PluginEngine.Lookups
{
    /// <summary>
    /// Describes registry of the Handbook Lookup Data Providers
    /// </summary>
    public interface IHandbookDataProviderRegistry
    {
        /// <summary>
        /// Gets the data provider by name
        /// </summary>
        /// <param name="providerName">Data provider name</param>
        ILookupDataProvider GetProvider(string providerName);

        /// <summary>
        /// Gets the collection of the all available Lookup data providers
        /// </summary>
        IEnumerable<ILookupDataProvider> GetAll();
    }
}
