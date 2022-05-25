using System;
using FilingPortal.PluginEngine.Common;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.PluginEngine.Lookups
{
    /// <summary>
    /// Represents the registry of the handbook data providers
    /// </summary>
    internal class HandbookDataProviderRegistry : IHandbookDataProviderRegistry
    {
        /// <summary>
        /// The data providers dictionary
        /// </summary>
        private readonly Dictionary<string, ILookupDataProvider> _dataProviders;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandbookDataProviderRegistry"/> class
        /// </summary>
        /// <param name="dataProviders">Data provider collection</param>
        public HandbookDataProviderRegistry(IEnumerable<IUiAvailable> dataProviders)
        {
            _dataProviders = dataProviders.OfType<ILookupDataProvider>()
                .GroupBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the data provider by name
        /// </summary>
        /// <param name="providerName">Data provider name</param>
        public ILookupDataProvider GetProvider(string providerName)
        {
            if (_dataProviders.ContainsKey(providerName))
            {
                return _dataProviders[providerName];
            }
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Gets the collection of the all available Lookup data providers
        /// </summary>
        public IEnumerable<ILookupDataProvider> GetAll() => _dataProviders.Values.ToList().AsReadOnly();
    }
}