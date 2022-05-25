using System;

namespace FilingPortal.PluginEngine.GridConfigurations.Filters
{
    /// <summary>
    /// Interface for registry of FilterDataProviders
    /// </summary>
    public interface IFilterDataProviderRegistry
    {
        /// <summary>
        /// Gets the provider by field name
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        IFilterDataProvider GetProvider(string fieldName);
        /// <summary>
        /// Gets the provider
        /// </summary>
        /// <param name="dataSourceType">Type of the data source</param>
        IFilterDataProvider GetProvider(Type dataSourceType);
    }
}