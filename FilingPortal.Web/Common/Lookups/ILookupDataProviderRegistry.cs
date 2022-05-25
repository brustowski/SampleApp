using System;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups
{
    /// <summary>
    /// Describes registry of the Lookup Data Providers
    /// </summary>
    public interface ILookupDataProviderRegistry
    {
        /// <summary>
        /// Gets the provider
        /// </summary>
        ILookupDataProvider GetProvider<T>() where T : ILookupDataProvider;

        /// <summary>
        /// Gets the provider by type
        /// </summary>
        ILookupDataProvider GetProvider(Type dataProvider);

        /// <summary>
        /// Gets the data provider by name
        /// </summary>
        /// <param name="providerName">Data provider name</param>
        ILookupDataProvider GetProvider(string providerName);
    }
}
