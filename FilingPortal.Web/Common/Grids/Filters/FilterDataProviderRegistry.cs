using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.PluginEngine.GridConfigurations.Filters;

namespace FilingPortal.Web.Common.Grids.Filters
{
    /// <summary>
    /// Registry of FilterDataProviders
    /// </summary>
    internal class FilterDataProviderRegistry : IFilterDataProviderRegistry
    {
        /// <summary>
        /// The data providers
        /// </summary>
        private readonly Dictionary<string, IFilterDataProvider> _dataProviders = new Dictionary<string, IFilterDataProvider>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterDataProviderRegistry"/> class
        /// </summary>
        /// <param name="dataProviders">The data providers</param>
        public FilterDataProviderRegistry(IEnumerable<IFilterDataProvider> dataProviders)
        {
            foreach (IFilterDataProvider provider in dataProviders)
            {
                string name = provider.GetType().FullName;
                if (!_dataProviders.ContainsKey(name))
                    _dataProviders.Add(name, provider);
            }
        }

        /// <summary>
        /// Gets the provider by field name
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        public IFilterDataProvider GetProvider(string fieldName)
        {
            var provider = _dataProviders.Values.FirstOrDefault(x => x.GetType().Name == fieldName);
            return provider ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Gets the provider by specified type
        /// </summary>
        /// <param name="dataSourceType">Type of the data source</param>
        public IFilterDataProvider GetProvider(Type dataSourceType)
        {
            string name = dataSourceType.FullName;

            if (_dataProviders.ContainsKey(name))
            {
                return _dataProviders[name];
            }
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Gets all FilterDataProviders
        /// </summary>
        public IEnumerable<IFilterDataProvider> GetAll()
        {
            return _dataProviders.Values;
        }
    }
}