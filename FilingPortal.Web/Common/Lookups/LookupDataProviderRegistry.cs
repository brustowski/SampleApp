using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups
{
    /// <summary>
    /// Registry of FilterDataProviders
    /// </summary>
    internal class LookupDataProviderRegistry : ILookupDataProviderRegistry
    {
        /// <summary>
        /// The data providers
        /// </summary>
        private readonly Dictionary<string, ILookupDataProvider> _dataProviders;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDataProviderRegistry"/> class
        /// </summary>
        /// <param name="dataProviders">Data provider collection</param>
        public LookupDataProviderRegistry(IEnumerable<ILookupDataProvider> dataProviders)
        {
            if (dataProviders != null)
            {
                _dataProviders = dataProviders
                    .GroupBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
            }

        }

        /// <summary>
        /// Gets the data provider by specified type
        /// </summary>
        /// <exception cref="ArgumentException">Requested type is not registered</exception>
        public ILookupDataProvider GetProvider<T>()
            where T : ILookupDataProvider => GetProvider(typeof(T));

        /// <summary>
        /// Gets data provider by its type
        /// </summary>
        /// <param name="dataProviderType">Type of the data provider</param>
        public ILookupDataProvider GetProvider(Type dataProviderType)
        {
            ILookupDataProvider result = _dataProviders.Values.FirstOrDefault(x => dataProviderType == x.GetType());

            return result ?? throw new KeyNotFoundException();
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
    }
}